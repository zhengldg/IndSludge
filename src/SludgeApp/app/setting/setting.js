import React, { Component } from 'react';
import { View, Alert, Text, AsyncStorage, TouchableOpacity, Dimensions, TouchableHighlight, StyleSheet } from 'react-native';
import { Container, Button, Header, Toast, Content, List, ListItem, Left, Body, Right, Switch } from 'native-base';
import Icon from 'react-native-vector-icons/FontAwesome';
import { userMgr, tokenMgr, AsyncStore } from '../common/util'

export default class Setting extends Component {
    constructor(prpos) {
        super(prpos);
        this.state = {
            user: {}
        }
    }

    changePassword = () => {
        this.props.navigation.navigate("ChangePassword");
    }

    onLocation = () => {
        this.props.navigation.navigate("Location");
    }

    clearCache = () => {
        var self = this;
        Alert.alert(
            '提醒',
            '确定要清理缓存吗',
            [
                { text: '取消', onPress: () => console.log('Cancel Pressed') },
                {
                    text: '确定', onPress: () => {
                        Toast.show({ type: 'success', position: 'top', text: '清理成功' });
                    }
                },
            ],
            { canceLabel: true }
        )
    }

    about = () => {
        Alert.alert(
            '关于',
            '版本号：V1.0.1',
            [
                {
                    text: '确定', onPress: () => {
                    }
                },
            ],
            { canceLabel: true }
        )
    }

    componentDidMount = () => {
        userMgr.getCurrent().then(x => {
            if (x) {
                this.setState({
                    user: x
                })
            } else {
                this.props.navigation.navigate("Login");
            }
        })
    }

    logout = () => {
        var self = this;
        Alert.alert(
            '提醒',
            '确定要退出应用吗',
            [
                {
                    text: '取消', onPress: () => {
                    }
                },
                {
                    text: '确定', onPress: () => {
                        userMgr.clearCurrentUserData();
                        self.props.navigation.navigate('Login');
                    }
                },

            ],
            { canceLabel: true }
        )
    }

    render() {
        return (
            <View style={{ flex: 1, justifyContent: 'flex-start' }} >
                <View style={{ backgroundColor: '#096dd9', height: 180, justifyContent: 'center', alignItems: 'center' }}>
                    <View style={{ alignItems: 'center' }}>
                        <Icon size={66} name="user-circle" color='#fff'>
                        </Icon>
                        <Text style={{ color: '#fff', marginTop: 10 }}>{this.state.user.name}</Text>
                    </View>
                </View>
                <View style={{ marginTop: 15 }}>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="lock" size={30} color='#ffa940' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5} style={styles.right} onPress={this.changePassword}>
                            <Text style={styles.label}>修改密码</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="map-marker" size={30} color='#1890ff' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5} style={styles.right} onPress={this.onLocation}>
                            <Text style={styles.label}>我的位置</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="trash-o" size={30} color='#73d13d' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5} style={styles.right} onPress={this.clearCache}>
                            <Text style={styles.label}>清理缓存</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="info-circle" size={30} color='#006d75' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5} style={styles.right} onPress={this.about}>
                            <Text style={styles.label}>关于</Text>
                        </TouchableOpacity>
                    </View>
                </View>
                <View style={{ marginTop: 80, borderRadius: 10, height: 50, justifyContent: 'center', borderColor: '#bfbfbf', borderStyle: 'solid', borderWidth: StyleSheet.hairlineWidth }} >
                    <TouchableOpacity style={{ flex: 1, justifyContent: 'center' }} activeOpacity={0.5} onPress={this.logout} >
                        <Text style={{ fontSize: 18, color: '#40a9ff', alignSelf: 'center' }}>安全退出</Text>
                    </TouchableOpacity>
                </View>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    item: {
        flexDirection: 'row',
        alignItems: 'center',
        paddingTop: 12,
        paddingBottom: 10,
        borderBottomColor: '#bfbfbf', borderStyle: 'solid', borderBottomWidth: StyleSheet.hairlineWidth
    },
    left: {
        justifyContent: 'center', alignItems: 'center', width: 60, alignItems: 'center', justifyContent: 'center'
    },
    right: {
        flex: 1
    },
    label: {
        fontSize: 16
    }
})