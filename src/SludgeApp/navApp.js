import Login from './app/account/login'
import Register from './app/account/register'
import Home from './app/home/index'
import Companys from './app/base/companys'
import EleDuplicateAddOrEdit from './app/eleDuplicate/addoredit'

import { StackNavigator } from 'react-navigation'
var NavApp = StackNavigator({
  Login: {
    screen: Login,
    navigationOptions: {
      header: null
    }
  },
  Register: {
    screen: Register,
    navigationOptions: {
      title: '注册新账号'
    }
  },
  Home: {
    screen: Home,
    navigationOptions: {
      header: null
    }
  },
  Companys: {
    screen: Companys,
    navigationOptions: {
      header: null
    }
  },
  EleDuplicateAddOrEdit: {
    screen: EleDuplicateAddOrEdit,
    navigationOptions: {
      title: '发起转移联单'
    }
  }
},
  {
    initialRouteName: 'Login',
    navigationOptions: {
      headerBackTitle: '返回',
      headerStyle: {
        backgroundColor: '#108ee9',
      },
      headerTintColor: '#fff',
      headerTitleStyle: {
      }
    }
  })

export default NavApp;