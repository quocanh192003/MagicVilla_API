
using static MagicVilla_Utility.SD;
namespace MagicVilla_Web.Models;

public class APIRequest{
    public ApiType apiType { get; set; } = ApiType.GET;
    public string Url {get; set;}
    public object data {get; set;}
    public string Token {get; set;}
}