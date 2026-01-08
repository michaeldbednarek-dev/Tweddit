package com.babymonitor.identity.services;

import org.keycloak.OAuth2Constants;
import org.keycloak.admin.client.Keycloak;
import org.keycloak.admin.client.KeycloakBuilder;
import org.keycloak.admin.client.resource.RealmResource;
import org.keycloak.admin.client.resource.UsersResource;
import org.keycloak.representations.idm.CredentialRepresentation;
import org.keycloak.representations.idm.UserRepresentation;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;

import org.keycloak.admin.client.resource.UserResource;
import javax.ws.rs.core.Response;
import java.util.Collections;

@Service
public class UserCreation {

    @Value("${KEYCLOAK_AUTH_SERVER_URL}")
    private String authServerUrl;

    @Value("${KEYCLOAK_REALM}")
    private String realm;

    @Value("${KEYCLOAK_CLIENT_ID}")
    private String clientId;

    @Value("${KEYCLOAK_USER_ADMIN}")
    private String userAdmin;

    @Value("${KEYCLOAK_USER_PASSWORD}")
    private String userPassword;



    private Keycloak getKeycloakClient() {
        return KeycloakBuilder.builder()
                .serverUrl(authServerUrl)
                .realm(realm)
                .clientId(clientId)
                .username(userAdmin)
                .password(userPassword)
                .grantType(OAuth2Constants.PASSWORD)
                .build();
    }

    public String deleteUser(String userId) {
        try {
            Keycloak keycloak = getKeycloakClient();
            RealmResource realmResource = keycloak.realm(realm);
            String cleanedUserId = userId.replaceAll("=+$", "").trim();
            UserResource userResource = realmResource.users().get(cleanedUserId);

            String nameSpecial = "fail";
            nameSpecial = userResource.toRepresentation().getUsername(); //getAttributes().get("preferred_username").get(0);

            userResource.remove();
            return nameSpecial;
        } catch (Exception e) {
            e.printStackTrace();
            return "fail";
        }
    }

    public String createUser(String username, String email, String password) {
            Keycloak keycloak = getKeycloakClient();
            RealmResource realmResource = keycloak.realm(realm);
            UsersResource usersResource = realmResource.users();

            UserRepresentation user = new UserRepresentation();
            user.setUsername(username);
            user.setEmail(email);
            user.setFirstName("n/a");
            user.setLastName("n/a");
            user.setEnabled(true);

            CredentialRepresentation cred = new CredentialRepresentation();
            cred.setType(CredentialRepresentation.PASSWORD);
            cred.setValue(password);
            cred.setTemporary(false);

            user.setCredentials(Collections.singletonList(cred));

            Response response = usersResource.create(user);
            if (response.getStatus() == 201) {
                String userId = response.getLocation().getPath().replaceAll(".*/([^/]+)$", "$1");
                response.close();
                return userId;
            } else {
                System.err.println("Fout bij het aanmaken van de gebruiker:" + response.getStatusInfo());
                response.close();
                return null;
            }
    }
}
