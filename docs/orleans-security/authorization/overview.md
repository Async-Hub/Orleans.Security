# Overview

Authorization refers to the process that determines what a user is able to do. For example, an administrative user is allowed to create a document library, add documents, edit documents, and delete them. A non-administrative user working with the library is only authorized to read the documents.
Authorization is orthogonal and independent from authentication. However, authorization requires an authentication mechanism. Authentication is the process of ascertaining who a user is. Authentication may create one or more identities for the current user.

\* *Authorization in Microsoft Orleans is mainly the same ASP.NET Core authorization. You can read more about ASP.NET Core authorization [here](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-2.1).*