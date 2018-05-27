import { AsyncStorage, Alert } from 'react-native';
import { Toast as NToast } from 'native-base'
import moment from 'moment'
import _ from 'lodash'
import { get } from './request'

const TOKEN_KEY = "TOKEN_KEY";
const USER_KEY = "USER_KEY"

export const Toast = {
    defaultOption: {
        position: 'top'
    },
    success(options) {
        NToast.show(Object.assign(this.defaultOption, { type: 'success' }, options))
    },
    warn(options) {
        Alert.alert('提醒', options.text);
        // NToast.show(Object.assign(this.defaultOption, { type: 'warning' }, options))
    },
    danger(options) {
        Alert.alert('提醒', options.text);
    }
}

export const Alt = {
    info(message, buttons, options, type) {
        Alert.alert('提示', message, buttons, options, type)
    },
    error(message) {
        Alert.alert('发生错误', message, buttons, options, type)
    }
}

export const AsyncStore = {
    getItem(key) {
        return AsyncStorage.getItem(key);
    },
    setItem(key, item) {
        return AsyncStorage.setItem(key, item);
    },
    getObj(key) {
        return AsyncStorage.getItem(key).then(x => x == null ? null : JSON.parse(x));
    },
    setObj(key, item) {
        var itemJson = JSON.stringify(item);
        return AsyncStorage.setItem(key, itemJson);
    }
}

export const userMgr = {
    getCurrent() {
        return AsyncStorage.getItem(USER_KEY).then(x => { JSON.parse(x) })
    },
    setCurrent(user) {
        return AsyncStorage.setItem(USER_KEY, JSON.stringify(user));
    }
}

export const tokenMgr = {
    clear() {
        AsyncStorage.clear(TOKEN_KEY);
    },
    setToken(strToken) {
        var json = JSON.parse(strToken)
        json.expires_end = moment()
            .add(json.expires_in - 10, 's')
            .format();
        AsyncStorage.setItem(TOKEN_KEY, JSON.stringify(json))
    },
    getToken() {
        return new Promise(function (resolve, reject) {
            AsyncStorage
                .getItem(TOKEN_KEY)
                .then(x => {
                    if (x) {
                        var json = JSON.parse(x);
                        if (moment(json.expires_end).isBefore(moment())) {
                            resolve(null);
                        } else {
                            if (moment(json.expires_end).isBefore(moment().add(600, 's'))) { // 快过期了
                                _refreshToken(json)
                            }
                            resolve(json)
                        }
                    } else {
                        resolve(null)
                    }
                })
                .catch(x => {
                    reject(x);
                })
        })
    },
    _refreshToken(oldJson) {
        get('token/auth?grant_type=refresh_token&refresh_token=' + oldJson.refresh_token).then(x => {
            if (x.success) {
                this.setToken(x.data);
            } else {
                console.log('刷新token失败', x)
            }
        })
    }
}