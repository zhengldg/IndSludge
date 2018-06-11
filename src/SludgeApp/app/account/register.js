import React, { Component } from 'react';
import { Platform, AsyncStorage, StyleSheet, View, Text, Image, Alert, Dimensions, ActivityIndicator, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Item, Button, Input, Icon, ListItem, CheckBox, Body, Label, Right, Form } from 'native-base';
import Finished from '../base/finished'
import { get, post } from '../common/request'
import { tokenMgr, userMgr, AsyncStore, Alt, Toast } from '../common/util'
import Companys from '../base/companys'

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
            !finished ? <RegisterComp finished={this._finished} /> : <Finished btnText='返回到登录页面' clickFinished={this._goLogin} tip='注册成功' />
        );
    }
}

class RegisterComp extends Component {
    timer = null;

    constructor(props) {
        super(props)
        this.state = {
            PhoneNumber: '',
            Name: '',
            Email: '',
            CompanyInfoId: '',
            companyInfoName: '',
            VerifyCode: '',
            Password: '',
            loading: false,
            seconds: 0,
            isModalVisible: false
        }
    }

    componentDidMount = async () => {
        var user = await AsyncStore.getObj('remembeUser');
        if (user) {
            this.setState(user)
        }
    }

    _login = () => {
        if (!this.state.PhoneNumber) {
            Toast.warn({
                text: '请输入手机号码'
            })
            return;
        }
        if (!this.state.VerifyCode) {
            Toast.warn({
                text: '请输入验证码'
            })
            return;
        }
        if (!this.state.Name) {
            Toast.warn({
                text: '请输入姓名'
            })
            return;
        }
        if (!this.state.Email) {
            Toast.warn({
                text: '请输入邮箱'
            })
            return;
        }
        if (!this.state.CompanyInfoId) {
            Toast.warn({
                text: '请选择企业'
            })
            return;
        }
        if (!this.state.Password) {
            Toast.warn({
                text: '请输入密码'
            })
            return;
        }
        this.setState({ loading: true });
        post('u/register', {
            PhoneNumber: this.state.PhoneNumber,
            VerifyCode: this.state.VerifyCode,
            Name: this.state.Name,
            Email: this.state.Email,
            CompanyInfoId: this.state.CompanyInfoId,
            Password: this.state.Password,
        }).then(x => {
            if (x.success) {
                this.setState({ loading: false });
                this.props.finished && this.props.finished();
            } else {
                Alert.alert('注册失败', x.message);
            }
        }).catch(x => {
            Toast.danger('登录失败，请检查网络连接是否正常');
        }).done(x => {
            if (this.loading) {
                this.setState({ loading: false });
            }
        });
    }

    _selectCompany = () => {
        this.setState({ isModalVisible: true })
    }

    _onSelectedCompany = (item) => {
        this.setState({ CompanyInfoId: item.id, companyInfoName: item.name })
    }

    _getVerifyCode = () => {
        this.setState({
            seconds: 60
        })
        timer = setInterval(x => {
            var t = this.state.seconds;
            t--;
            this.setState({
                seconds: t
            })
            if (t == 0) {
                clearInterval(timer);
            }
        }, 1000);
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
                            <Label style={styles.label}>手机号码：</Label>
                            <Input placeholder="请输入手机号码" value={this.state.PhoneNumber} keyboardType={'numeric'} onChangeText={(txt) => {
                                this.setState({ PhoneNumber: txt })
                            }} />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>验证码：</Label>
                            <Input placeholder="请输入验证码" value={this.state.VerifyCode} keyboardType={'numeric'} onChangeText={(txt) => {
                                this.setState({ VerifyCode: txt })
                            }} />
                            <Button onPress={this._getVerifyCode} disabled={this.state.seconds > 0} size='small' success style={styles.verifyCodeBtn}>
                                {this.state.seconds > 0 ?
                                    <Text style={{ color: '#d4380d' }}>{this.state.seconds}秒</Text>
                                    :
                                    <Text style={{ color: '#d4380d' }}>获取</Text>
                                }
                            </Button>
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>姓名：</Label>
                            <Input placeholder="请输入姓名" value={this.state.Name} onChangeText={(txt) => {
                                this.setState({ Name: txt })
                            }} />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>邮箱：</Label>
                            <Input placeholder="请输入邮箱" value={this.state.Email} onChangeText={(txt) => {
                                this.setState({ Email: txt })
                            }} />
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>所在企业：</Label>
                            <Companys style={{ flex: 1 }} text={this.state.companyInfoName} onSelected={this._onSelectedCompany}></Companys>
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>密码：</Label>
                            <Input placeholder="请输入密码" value={this.state.Password} onChangeText={(txt) => {
                                this.setState({ Password: txt })
                            }} secureTextEntry={true} />
                        </Item>
                    </Form>
                    <Button disabled={this.state.loading} block rounded style={styles.loginBtn} onPress={this._login}>
                        <Text>注册</Text>
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
    loginBtn: { marginTop: 40, marginHorizontal: 40 },
    item: {
    },
    formContainer: {
    },
})
