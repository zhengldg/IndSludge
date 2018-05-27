import queryString from 'query-string'
import { Alert } from 'react-native';
import { AsyncStorage } from 'react-native';
import _ from 'lodash'
import config from './config'
import { tokenMgr } from './util'
import axios from 'axios'

export function get(url, params) {
    url = config.api.base + url
    return tokenMgr.getToken().then(function (token) {
        return axios({
            method: 'get',
            url: url,
            params: params,
            headers: {
                Authorization: 'Bearer ' + (token && token.access_token)
            }
        })
            .then(function (response) {
                console.log('************post:', response);
                return response.data;
            })
            .catch(function (error) {
                var response = error.response;
                return handErrorResponse(response);
            });
    });
}

function handErrorResponse(reaponse) {
    if (response.status == 400) {
        Alert.alert(response.data.message || '请求无效');
        return response.data || {}
    }
    else if (response.status == 404) {
        Alert.alert(response.data.message || '访问的资源不存在');
        return response.data || {}
    }
    else {
        Alert.alert(response.data.message || '服务器发生错误，请稍后再试');
        return response.data || {}
    }
}

export function post(url, body) {
    url = config.api.base + url
    return tokenMgr.getToken().then(function (token) {
        return axios({
            method: 'post',
            url: url,
            data: body,
            headers: {
                Authorization: 'Bearer ' + (token && token.access_token)
            }
        })
            .then(function (response) {
                console.log('************post:', response);
                return response.data;
            })
            .catch(function (error) {
                var response = error.response;
                return handErrorResponse(response);
            });
    });
}

export function upload(url, params) {
    url = config.api.base + url
    var formData = new FormData();
    if (params) {
        for (var p in params) {
            formData.append(p, params[p]);
        }
    }

    return new Promise((resolve, reject) => {
        var xhr = new XMLHttpRequest()
        var token = tokenMgr.getToken();
        // var options = {
        //     method: 'POST',
        //     headers: {
        //         'Accept': 'application/json',
        //         'Content-Type': 'multipart/form-data;charset=utf-8'
        //     }, body:
        //         formData
        // }
        if (token) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + token.access_token)
            // options.headers.Authorization = 
        }

        xhr.open('POST', url)
        xhr.onload = () => {
            if (xhr.status != 200) {
                reject(xhr.status)
            }

            if (!xhr.responseText) {
                reject(xhr.status)
            }

            var response
            try {
                response = JSON.parse(xhr.responseText)
                resolve(response)
            } catch (e) {
                reject(e)
            }
        }

        xhr.send(formData)
    });

}