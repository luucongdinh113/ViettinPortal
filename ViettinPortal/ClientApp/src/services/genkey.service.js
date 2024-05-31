import { handleResponse } from '../helpers/handle-response.js';

export const genkeyservice = {
    getApplicationList,
    getApplicationClientList,
    generateKey,
    generateClientKey,
    genKeyInstrument,
};

function getApplicationList(filter, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            ApplicationFilter: filter
        })
    };

    return fetch(`api/GenKey/GetApplicationList`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {

                //console.log("api/GenKey/GetApplicationList success: ");
                console.log(result);
                return result;
            },
            (error) => {
                //var objResult = {
                //    result: null,
                //    error: error,
                //};

                //console.log("Error processing !");
                console.log(error);
                return error;
            });
}

function getApplicationClientList(filter, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            ApplicationFilter: filter
        })
    };

    return fetch(`api/GenKey/GetApplicationClientList`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {

                //console.log("api/GenKey/GetApplicationClientList success: ");
                console.log(result);
                return result;
            },
            (error) => {

                //console.log("Error processing !");
                console.log(error);
                return error;
            });
}

function generateKey(generateKeyId, serial, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            GenerateKeyId: generateKeyId,
            Serial: serial,
        })
    };

    return fetch(`api/GenKey/Generate`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {

                //console.log("api/GenKey/GetApplicationClientList success: ");
                console.log(result);
                return result;
            },
            (error) => {

                //console.log("Error processing !");
                console.log(error);
                return error;
            });
}

function genKeyInstrument(generateKeyId, instrumentId, instrumentExpire, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            GenerateKeyId: generateKeyId,
            InstrumentId: instrumentId,
            InstrumentExpire: instrumentExpire
        })
    };

    return fetch(`api/GenKey/GenKeyInstrument`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {

                console.log(result);
                return result;
            },
            (error) => {

                console.log(error);
                return error;
            });
}

function generateClientKey(generateKeyId, serialClient, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            GenerateKeyId: generateKeyId,
            Serial: serialClient,
        })
    };

    return fetch(`api/GenKey/GenerateClient`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {

                //console.log("api/GenKey/GetApplicationClientList success: ");
                console.log(result);
                return result;
            },
            (error) => {

                //console.log("Error processing !");
                console.log(error);
                return error;
            });
}