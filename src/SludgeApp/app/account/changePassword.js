import React, { Component } from 'react';
import { Platform, AsyncStorage, StyleSheet, View, Text, Image, Alert, Dimensions, ActivityIndicator, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Item, Button, Input, Icon, ListItem, CheckBox, Body, Label, Right, Form } from 'native-base';
import Finished from '../base/finished'
import { get, post } from '../common/request'
import { tokenMgr, userMgr, AsyncStore, Alt, Toast } from '../common/util'

export default class Register extends Component {
    constructor(props) {
        super(props)
        this.state = {
            finished: false
        }
    }

    _goLogin = () => {
        this.props.navigation.navigate('Login');
    }

    _finished = () => {
        this.setState({ finished: true })
    }

    render() {
        var finished = this.state.finished;
        return (
            <RegisterComp navigation={this.props.navigation} finished={this._finished} />
        );
    }
}

class RegisterComp extends Component {
    timer = null;

    constructor(props) {
        super(props)
        this.state = {
            currentPassword: '',
            Name: '',
            Email: '',
        }
    }


    _changePassword = () => {
        if (!this.state.currentPassword) {
            Toast.warn({
                text: '请输入密码'
            })
            return;
        }
        if (!this.state.newPassword) {
            Toast.warn({
                text: '请输入新密码'
            })
            return;
        }
        if (!this.state.confirmPassword) {
            Toast.warn({
                text: '请再次输入新密码'
            })
            return;
        }
        if (this.state.confirmPassword != this.state.newPassword) {
            Toast.warn({
                text: '新密码与确认密码不一致'
            })
            return;
        }
        this.setState({ loading: true });

        post('u/changePassword', {
            currentPassword: this.state.currentPassword,
            newPassword: this.state.newPassword,
            confirmPassword: this.state.confirmPassword,
        }).then(x => {
            if (x.success) {
                this.setState({ loading: false });
                var self = this;
                Alert.alert(
                    '提醒',
                    '密码修改成功',
                    [
                        {
                            text: '确定', onPress: () => {
                                userMgr.getCurrent().then(x => {
                                    if (x) {
                                        return AsyncStore.setObj(userMgr.getRemembeKey(), { userName: x.userName });
                                    } else {
                                        return AsyncStorage.removeItem(userMgr.getRemembeKey());
                                    }
                                }).done(x => {
                                    userMgr.clearCurrentUserData();
                                    self.props.navigation.navigate('Login');
                                })
                            }
                        },

                    ],
                    { canceLabel: true }
                )
            } else {
                Alert.alert('修改失败', x.message);
            }
        }).catch(x => {
            Toast.danger('修改失败，请检查网络连接是否正常');
        }).done(x => {
            if (this.state.loading) {
                this.setState({ loading: false });
            }
        });
    }


    render() {
        return (
            <Container >
                <Content style={styles.formContainer}>
                    {
                        this.state.loading && <ActivityIndicator size="large" style={{ position: 'absolute', alignSelf: 'center' }} />
                    }
                    <Form style={{ marginRight: 15 }}>
                        <Item style={styles.item} >
                            <Label style={styles.label}>当前密码：</Label>
                            <Input placeholder="请输入当前密码" value={this.state.currentPassword} keyboardType={'numeric'} onChangeText={(txt) => {
                                this.setState({ currentPassword: txt })
                            }} secureTextEntry={true} />
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>新密码：</Label>
                            <Input placeholder="请输入新密码" value={this.state.newPassword} onChangeText={(txt) => {
                                this.setState({ newPassword: txt })
                            }} secureTextEntry={true} />
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>新密码确认：</Label>
                            <Input placeholder="请再次输入新密码" value={this.state.confirmPassword} onChangeText={(txt) => {
                                this.setState({ confirmPassword: txt })
                            }} secureTextEntry={true} />
                        </Item>
                    </Form>
                    <Button disabled={this.state.loading} block rounded style={styles.loginBtn} onPress={this._changePassword}>
                        <Text style={{ color: '#fff' }}>确  定</Text>
                    </Button>
                </Content>
            </Container>);
    }
}

const styles = StyleSheet.create({
    verifyCodeBtn: {
        marginRight: 5,
        borderColor: '#d4380d'
        , borderWidth: StyleSheet.hairlineWidth
        , borderStyle: 'solid'
        , backgroundColor: 'transparent'
        , width: 70, height: 32, justifyContent: 'center', alignSelf: 'center'
    }
    , label: {
        fontWeight: 'bold'
    },
    loginBtn: { marginTop: 40, marginHorizontal: 40, backgroundColor: '#1890ff' },
    item: {
    },
    formContainer: {
    },
})
