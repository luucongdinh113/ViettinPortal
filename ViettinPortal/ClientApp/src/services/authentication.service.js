import { BehaviorSubject } from 'rxjs';

//import config from 'config';
import { handleResponse } from '../helpers/handle-response.js';

const currentUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('currentUser')));

export const authenticationService = {
    login,
    logout,
    currentUser: currentUserSubject.asObservable(),
    get currentUserValue() { return currentUserSubject.value }
};

function login(email, password) {
    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            Email: email,
            Password: password,
        })
    };

    return fetch(`api/Account/authenticate`, requestOptions)
        .then(handleResponse)
        .then(
            (result) => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(result));
                currentUserSubject.next(result);
                //console.log(JSON.stringify(user));

                var objResult = {
                    result: result,
                    error: null, 
                };

                console.log("Account/LoginSubmit success: ");
                console.log(objResult);
                return objResult;
            },
            (error) => {
                var objResult = {
                    result: null,
                    error: error,
                };

                console.log("Error processing !");
                console.log(objResult);
                return objResult;
            });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    currentUserSubject.next(null);
}