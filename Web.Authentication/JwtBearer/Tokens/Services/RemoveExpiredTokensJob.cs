namespace Web.Authentication.JwtBearer.Tokens.Services;

[DisallowConcurrentExecution]
public class RemoveExpiredTokensJob(ILiteData lite, TimeProvider tp): IJob {
   public static readonly JobKey JobKey = new(name: "RemoveExpiredTokensJob", group: "RemoveTokens");
   public static readonly TriggerKey TriggerKey = new(name: "RemoveTokensTrigger", group: "TokensPool");

   public Task Execute(IJobExecutionContext context) => (
      from clock in IO.lift(tp.GetLocalNow().ToLocalTime)
      // Need to concert the IEnumerable or else exception throwed as the snapshot is disposed(LanguageExt)
      let refreshes = append(
         [.. lite.Find<Refresh>(r => r.IsFault)],
         [.. lite.Find<Refresh>(r => r.ExpiryDate <= clock)]
      )
      from _delete in lite.Atomic(
         c => () => iter(refreshes, x => iter(x.Accesses, a => c.Delete<Access>(a.Id))),
         c => () => iter(refreshes, x => c.Delete<Refresh>(x.Id))
      )
      select unit
   ).RunAsync().ToRef();
}