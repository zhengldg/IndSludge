import React, { Component } from 'react';
import { Platform, AsyncStorage, StyleSheet, View, Text, Image, Alert, Dimensions, ActivityIndicator } from 'react-native';
import { Container, Header, Content, Item, Button, Input, Icon, ListItem, CheckBox, Body, Label, Right, Spinner } from 'native-base';
import { get } from '../common/request'
import { tokenMgr, userMgr, AsyncStore, Alt } from '../common/util'

const width = Dimensions
  .get('window')
  .width

export default class Login extends Component {
  constructor(props) {
    super(props)
    this.state = {
      userName: '',
      password: '',
      remembeMe: false,
      logining: false
    }
  }

  componentDidMount = async () => {
    var user = await AsyncStore.getObj(userMgr.getRemembeKey());
    if (user) {
      this.setState(user)
    }
  }

  _login = () => {

    if (!this.state.userName) {
      Alert.alert('请输入用户名');
      return;
    }
    if (!this.state.password) {
      Alert.alert('请输入密码');
      return;
    }
    this.setState({ logining: true });
    get('token/auth', {
      userName: this.state.userName,
      password: this.state.password,
      grant_type: 'password'
    }).then(x => {
      if (x.success) {

        tokenMgr.setToken(x.data)
        var memembeUser = { userName: this.state.userName }
        if (this.state.remembeMe) {
          memembeUser.password = this.state.password;
          memembeUser.remembeMe = true;
        }
        AsyncStore.setObj(userMgr.getRemembeKey(), memembeUser);
        get('u/info').then(rsp => {
          if (rsp.success) {
            userMgr.setCurrent(rsp.data)
            this.props.navigation.navigate('Home');
          } else {
            Alert.alert('登录失败', rsp.message);
          }
        })
      } else {
        Alert.alert('登录失败', x.message);
      }
    }).catch(x => {
      Alert.alert('登录失败，请检查网络连接是否正常');
    }).done(x => {
      this.setState({ logining: false });
    });
  }

  _register = () => {
    this.props.navigation.navigate('Register');
  }

  _toggleRemembeMe = () => {
    var rem = this.state.remembeMe;
    this.setState({ remembeMe: !rem })
  }

  render() {
    return (
      <Container >
        <Image
          style={styles.logo} resizeMode='contain'
          source={require('./img/logo.png')}
        />
        <Content style={styles.formContainer}>
          {
            this.state.logining && <ActivityIndicator size="large" style={{ position: 'absolute', alignSelf: 'center' }} />
          }

          <Item style={styles.item}>
            <Icon active name="user" type="FontAwesome" />
            <Input placeholder='请输入用户名' value={this.state.userName} onChangeText={(txt) => {
              this.setState({ userName: txt })
            }} />
          </Item>
          <Item style={styles.item}>
            <Icon active name='lock' type="FontAwesome" />
            <Input placeholder='请输入密码' value={this.state.password} onChangeText={(txt) => {
              this.setState({ password: txt })
            }} secureTextEntry={true} keyboardType={'numeric'} />
          </Item>
          <ListItem style={{ marginLeft: 0, paddingLeft: 40 }}>
            <CheckBox checked={this.state.remembeMe} onPress={this._toggleRemembeMe} />
            <Body>
              <Text onPress={this._toggleRemembeMe}>记住密码</Text>
            </Body>
            <Right >
              <Body>
                <Text onPress={this._register}>注册新账号</Text>
              </Body>
            </Right>
          </ListItem>
          <Button disabled={this.state.logining} block rounded style={styles.loginBtn} onPress={this._login}>
            <Text style={{ color: '#fff' }}>登  录</Text>
          </Button>
        </Content>
        <Image
          style={styles.bg}
          source={require('./img/bg.png')}
        />
      </Container>
    );
  }
}

const styles = StyleSheet.create({
  loginBtn: { marginTop: 60, marginHorizontal: 40, backgroundColor: '#1890ff' },
  bg: {
    width: width,
    height: 400
  },
  item: {
    paddingHorizontal: 40,
    paddingVertical: 5,
  },
  formContainer: {
    flex: 1,
    marginTop: 50,
  },
  logo: {
    justifyContent: 'center', alignSelf: 'center',
    width: 320,
    height: 180,
    marginTop: 40,
  },
  container: {
    flex: 1,
    justifyContent: 'flex-start',
    alignItems: 'center',
  }
})
