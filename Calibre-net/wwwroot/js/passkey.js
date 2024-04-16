window.blazorPasskey = {
    test: () => console.log("hello world")
    // , abortController: new AbortController()
    // , abortSignal: new AbortController().abortController.signal
    , canPassKey: () => {
        return new Promise((resolve, reject) => {
            // Availability of `window.PublicKeyCredential` means WebAuthn is usable.  
            // `isUserVerifyingPlatformAuthenticatorAvailable` means the feature detection is usable.  
            // `​​isConditionalMediationAvailable` means the feature detection is usable.  
            if (window.PublicKeyCredential &&
                PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable) {
                // Check if user verifying platform authenticator is available.  
                // PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable().then(result => {
                //     return result;
                // });
                // const test =  await PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable();
                // console.log(test);

                PublicKeyCredential.isUserVerifyingPlatformAuthenticatorAvailable().then(_ => {
                    console.log("isUserVerifyingPlatformAuthenticatorAvailable");
                    console.log(_);
                    resolve(_);
                });
                // return test;
            }
        });
    }
    , canConditionalUi: async () => {
        return new Promise((resolve, reject) => {

            // Availability of `window.PublicKeyCredential` means WebAuthn is usable.  
            if (window.PublicKeyCredential &&
                PublicKeyCredential.isConditionalMediationAvailable) {
                // Check if conditional mediation is available.  
                PublicKeyCredential.isConditionalMediationAvailable().then(_ => {
                    console.log("isConditionalMediationAvailable");
                    console.log(_);
                    resolve(_);
                });
            }
        });
    }
    , generatePassKey: async (makeCredentialOptions) => {

        console.log(makeCredentialOptions);



        // Turn the challenge back into the accepted format of padded base64
        makeCredentialOptions.challenge = blazorPasskey.coerceToArrayBuffer(makeCredentialOptions.challenge);
        // Turn ID into a UInt8Array Buffer for some reason
        makeCredentialOptions.user.id = blazorPasskey.coerceToArrayBuffer(makeCredentialOptions.user.id);

        makeCredentialOptions.excludeCredentials = makeCredentialOptions.excludeCredentials.map((c) => {
            c.id = blazorPasskey.coerceToArrayBuffer(c.id);
            return c;
        });

        if (makeCredentialOptions.authenticatorSelection.authenticatorAttachment === null) makeCredentialOptions.authenticatorSelection.authenticatorAttachment = undefined;

        console.log("Credential Options Formatted", makeCredentialOptions);
        try {
            const newCredential = await navigator.credentials.create({
                publicKey: makeCredentialOptions
            });
            console.log(newCredential);

            // Encode and send the credential to the server for verification.


            // Move data into Arrays incase it is super long
            let attestationObject = new Uint8Array(newCredential.response.attestationObject);
            let clientDataJSON = new Uint8Array(newCredential.response.clientDataJSON);
            let rawId = new Uint8Array(newCredential.rawId);

            const data = {
                id: newCredential.id,
                rawId: blazorPasskey.coerceToBase64Url(rawId),
                type: newCredential.type,
                extensions: newCredential.getClientExtensionResults(),
                response: {
                    AttestationObject: blazorPasskey.coerceToBase64Url(attestationObject),
                    clientDataJSON: blazorPasskey.coerceToBase64Url(clientDataJSON)
                }
            };
            // credential.rawId = new TextDecoder().decode(credential.rawId);

            console.log(data);

            return data;
        }
        catch (error) { console.error(error); return null; }
    }
    , coerceToArrayBuffer: function (thing, name) {
        if (typeof thing === "string") {
            // base64url to base64
            thing = thing.replace(/-/g, "+").replace(/_/g, "/");

            // base64 to Uint8Array
            var str = window.atob(thing);
            var bytes = new Uint8Array(str.length);
            for (var i = 0; i < str.length; i++) {
                bytes[i] = str.charCodeAt(i);
            }
            thing = bytes;
        }

        // Array to Uint8Array
        if (Array.isArray(thing)) {
            thing = new Uint8Array(thing);
        }

        // Uint8Array to ArrayBuffer
        if (thing instanceof Uint8Array) {
            thing = thing.buffer;
        }

        // error if none of the above worked
        if (!(thing instanceof ArrayBuffer)) {
            throw new TypeError("could not coerce '" + name + "' to ArrayBuffer");
        }

        return thing;
    }

    , coerceToBase64Url: function (thing) {
        // Array or ArrayBuffer to Uint8Array
        if (Array.isArray(thing)) {
            thing = Uint8Array.from(thing);
        }

        if (thing instanceof ArrayBuffer) {
            thing = new Uint8Array(thing);
        }

        // Uint8Array to base64
        if (thing instanceof Uint8Array) {
            var str = "";
            var len = thing.byteLength;

            for (var i = 0; i < len; i++) {
                str += String.fromCharCode(thing[i]);
            }
            thing = window.btoa(str);
        }

        if (typeof thing !== "string") {
            throw new Error("could not coerce to string");
        }

        // base64 to base64url
        // NOTE: "=" at the end of challenge is optional, strip it off here
        thing = thing.replace(/\+/g, "-").replace(/\//g, "_").replace(/=*$/g, "");

        return thing;
    }
    , getCredentials: async (credentialOptions) => {

        console.log(credentialOptions);

        // Turn the challenge back into the accepted format of padded base64
        credentialOptions.challenge = blazorPasskey.coerceToArrayBuffer(credentialOptions.challenge);

        credentialOptions.allowCredentials = credentialOptions.allowCredentials.map((c) => {
            c.id = blazorPasskey.coerceToArrayBuffer(c.id);
            return c;
        });
        console.log(credentialOptions);

        const credential = await navigator.credentials.get({
            publicKey: credentialOptions
        });

        console.log(credential);

        
            // Move data into Arrays incase it is super long
            let authenticatorData = new Uint8Array(credential.response.authenticatorData);
            let clientDataJSON = new Uint8Array(credential.response.clientDataJSON);
            let rawId = new Uint8Array(credential.rawId);
            let signature =  new Uint8Array(credential.response.signature);
            let userHandle = new Uint8Array(credential.response.userHandle);
            let attestationObject = new Uint8Array(credential.response.attestationObject);

            const data = {
                id: credential.id,
                rawId: blazorPasskey.coerceToBase64Url(rawId),
                type: credential.type,
                extensions: credential.getClientExtensionResults(),
                response: {
                    authenticatorData: blazorPasskey.coerceToBase64Url(authenticatorData),
                    clientDataJSON: blazorPasskey.coerceToBase64Url(clientDataJSON),
                    signature : blazorPasskey.coerceToBase64Url(signature),
                    userHandle:  blazorPasskey.coerceToBase64Url(userHandle),
                    attestationObject:  blazorPasskey.coerceToBase64Url(attestationObject),
                }
            };
            // credential.rawId = new TextDecoder().decode(credential.rawId);

            console.log(data);

            return data;

        return credential;

    }


};