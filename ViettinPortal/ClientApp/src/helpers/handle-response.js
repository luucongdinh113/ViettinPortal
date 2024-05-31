import { authenticationService } from '../services/authentication.service.js';

export function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if ([401, 403].indexOf(response.status) !== -1) {
                // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
                authenticationService.logout();

                // can not load ???
                //location.reload(true);
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });

    //if (!response.ok) {
    //    if ([401, 403].indexOf(response.status) !== -1) {
    //        //auto logout if 401 Unauthorized or 403 Forbidden response returned from api
    //        authenticationService.logout();
    //    }

    //    const data = response.text() && JSON.parse(response.text());
    //    const error = (data && data.message) || response.statusText;
    //    return Promise.reject(error);
    //    //return Promise.reject(response);
    //}
    //else {
    //    return JSON.parse(response.text());
    //}

    //return response;
}