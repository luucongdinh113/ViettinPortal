import { handleResponse } from '../helpers/handle-response.js';

export const customerService = {
    getCustomerList,
    deleteCustomer,
    addCustomer,
};

function getCustomerList(filter, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            CustomerIdFilter: filter.CustomerIdFilter, 
            CustomerNameFilter: filter.CustomerNameFilter, 
        })
    };

    return fetch(`api/Customer/GetCustomerList`, requestOptions)
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

function deleteCustomer(request, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            SysId: request.SysId,
        })
    };

    return fetch(`api/Customer/DeleteCustomer`, requestOptions)
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

function addCustomer(request, token) {

    const requestOptions = {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: JSON.stringify({
            CustomerId: request.CustomerId,
            CustomerName: request.CustomerName,
            Description: request.Description,
        })
    };

    return fetch(`api/Customer/AddCustomer`, requestOptions)
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