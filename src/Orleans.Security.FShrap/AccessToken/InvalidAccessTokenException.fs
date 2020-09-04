namespace Orleans.Security.AccessToken

open System

type InvalidAccessTokenException=
    inherit Exception
    new () = {}
    new (message) = {inherit Exception(message);}
    new (message : string, innerException) = {inherit Exception(message,innerException);}

//using System;

//// ReSharper disable UnusedMember.Global

//namespace Orleans.Security.AccessToken
//{
//    public class InvalidAccessTokenException : Exception
//    {
//        public InvalidAccessTokenException()
//        {
//        }

//        public InvalidAccessTokenException(string message) : base(message)
//        {
//        }

//        public InvalidAccessTokenException(string message, Exception innerException) : 
//            base(message, innerException)
//        {
//        }
//    }
//}