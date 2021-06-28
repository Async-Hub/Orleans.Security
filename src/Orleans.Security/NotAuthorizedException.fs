namespace Orleans.Security

open System

[<Serializable>]
type NotAuthorizedException =
    inherit Exception

    new() = {inherit Exception();}
    new(message) = { inherit Exception(message); }
    new(message, innerException : Exception) = {inherit Exception(message, innerException);}

//using System;

//namespace Orleans.Security
//{
//    [Serializable]
//    public class NotAuthorizedException : Exception
//    {
//        // ReSharper disable once UnusedMember.Global
//        public NotAuthorizedException()
//        {
//        }

//        public NotAuthorizedException(string message) : base(message)
//        {
//        }

//        public NotAuthorizedException(string message, Exception innerException) :
//            base(message, innerException)
//        {
//        }
//    }
//}