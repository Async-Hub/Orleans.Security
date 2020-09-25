namespace Orleans.Security

open System.Threading.Tasks

type IAccessTokenProvider =
    abstract member RetrieveTokenAsync : unit -> Task<string>

//using System.Threading.Tasks;

//namespace Orleans.Security
//{
//    public interface IAccessTokenProvider
//    {
//        Task<string> RetrieveTokenAsync();
//    }
//}