module GlobalConfig

open Orleans.Security

let identityServer4Url = "http://localhost:5001"

let identityServer4Info = IdentityServer4Info(identityServer4Url,
                                "Orleans", "@3x3g*RLez$TNU!_7!QW", "Orleans");
