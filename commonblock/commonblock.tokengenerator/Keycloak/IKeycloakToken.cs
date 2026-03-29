using System;

namespace commonblock.tokengenerator.Keycloak;

public interface IKeycloakToken
{
  Task<string> GetAccessTokenAsync();
}
