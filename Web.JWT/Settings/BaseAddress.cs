namespace Web.JWT.Settings;

public sealed record class BaseAddress(string DGS3, string SPV, string Host) {
   public BaseAddress()
      : this(DGS3: string.Empty, SPV: string.Empty, Host: string.Empty) { }
}