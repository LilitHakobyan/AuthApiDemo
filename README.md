# AuthApiDemo
Identity Server 4 implementation for Web Api

Identity Server 4 is the tool of choice for getting bearer JSON web tokens (JWT) in .NET. The tool comes in a NuGet package that can fit in any ASP.NET project. Identity Server 4 is an implementation of the OAuth 2.0 spec and supports standard flows. The library is extensible to support parts of the spec that are still in draft.

Bearer JWT tokens are preferable to authenticate requests with a backend API. The JWT is stateless and aids in decoupling software modules. The JWT itself is not tied to the user session and works well in a distributed system. This reduces friction between modules since it does not share dependencies like a user session.
