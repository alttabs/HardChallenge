using System;

public partial class OAuthIntegration
{
  public int Id { get; set; }
  public string Provider { get; set; }
  public string ClientId { get; set; }
  public string ClientSecret { get; set; }
  public DateTime CreatedOn { get; set; } = DateTime.Now;
}