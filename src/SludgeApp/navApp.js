import Login from './app/account/login'
import Register from './app/account/register'
import Home from './app/home/index'
import Companys from './app/base/companys'
import EleDuplicateAddOrEdit from './app/eleDuplicate/addoredit'
import Process from './app/eleDuplicate/process'
import TaskList from './app/eleDuplicate/tasklist'
import FinishedList from './app/eleDuplicate/finishedlist'
import EleDuplicateDetail from './app/eleDuplicate/detail'
import Setting from './app/setting/setting'

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
  },
  EleDuplicateProcess: {
    screen: Process,
    navigationOptions: {
      title: '转移联单接收'
    }
  },
  TaskList: {
    screen: TaskList,
    navigationOptions: {
      title: '待办联单'
    }
  },
  FinishedList: {
    screen: FinishedList,
    navigationOptions: {
      title: '已办结联单'
    }
  },
  EleDuplicateDetail: {
    screen: EleDuplicateDetail,
    navigationOptions: {
      title: '联单详情'
    }
  },
  Setting: {
    screen: Setting,
    navigationOptions: {
      title: '系统设置'
    }
  }
},
  {
    initialRouteName: 'Setting',
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