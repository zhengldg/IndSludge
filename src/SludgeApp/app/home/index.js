//import liraries
import React, { Component } from 'react';
import {
    View,
    Alert,
    Text,
    StyleSheet,
    Image,
    Dimensions,
    ScrollView,
    TouchableHighlight,
    TouchableOpacity
} from 'react-native';
import { Container, Header, Icon, Content, CardItem, List, ListItem, Left, Body, Right, Thumbnail, H3, Item, Input, Button, Card, Footer, Title } from 'native-base';
import { get, post } from '../common/request'
import moment from 'moment'

var width = Dimensions
    .get('window')
    .width, height = Dimensions
        .get('window')
        .height

// create a component
class Home extends Component {

    constructor(props) {
        super(props)

        this.state = {
            todoList: []
        }
    }

    componentDidMount() {
        post('eleDuplicate/homeMissions')
            .then(x => {
                this.setState({
                    todoList: x.data
                })
            })
    }

    _newManifest = () => {
        this.props.navigation.navigate('EleDuplicateAddOrEdit');
    }

    _editManifest = (item) => {
        if (item.state == '确认启运') {
            this.props.navigation.navigate('EleDuplicateProcess', { id: item.id });
        } else if (item.state == '经营单位退回') {
            this.props.navigation.navigate('EleDuplicateAddOrEdit', { id: item.id });
        }
    }

    renderToDoList = () => {
        var items = [];
        this.state.todoList.forEach(x => {
            items.push((
                <ListItem key={x.id} onPress={() => {
                    this._editManifest(x);
                }}>
                    <Thumbnail style={styles.itemThum} source={require('./img/icon_fqld.png')} />
                    <Body style={{ marginLeft: 5 }}>
                        <Text>联单号码：{x.code}</Text>
                        <Text note>发运时间：{moment(x.departureTime).format('YYYY-MM-DD HH:mm')}</Text>
                        <Text note>当前状态：{x.state}  转运数量(吨)：{x.quantity} </Text>
                        {
                            x.state == '经营单位退回'
                            && <Text style={{ color: '#f5222d', textAlign: 'left' }}>原因：{x.backReason}</Text>
                        }
                    </Body>
                </ListItem>));
        })
        return items;
    }
    render() {
        return (
            <Container>
                <Content>
                    <CardItem cardBody style={{ justifyContent: 'center' }}>
                        <Image source={require('./img/bg_home.png')} resizeMode='cover' style={{ flex: 1, height: 200 }} />
                        <Body style={{ position: 'absolute', alignItems: 'center', alignSelf: 'center' }} >
                            <Icon active name="file-text-o" type='FontAwesome' style={styles.iconNew} />
                            <Button onPress={this._newManifest} style={styles.btnNew}>
                                <Text style={styles.txtNew}>发起联单</Text></Button>
                        </Body>
                    </CardItem>
                    <View style={{ flexDirection: 'row', justifyContent: 'center', alignItems: 'center' }}>
                        <View style={styles.splitLine}></View><Text style={styles.title}> 待办联单 </Text>
                        <View style={styles.splitLine}></View>
                    </View>
                    <List style={{ height: height - 480 }} >
                        {
                            this.state.todoList && this.state.todoList.length > 0 ?
                                this.renderToDoList()
                                :
                                <View>
                                    <Text style={{ alignSelf: 'center', margin: 10 }}>无待办项</Text>
                                </View>
                        }
                    </List>
                    <View style={{ flexDirection: 'row', justifyContent: 'center', alignItems: 'center' }}>
                        <View style={styles.splitLine}></View><Text style={styles.title}> 系统菜单 </Text>
                        <View style={styles.splitLine}></View>
                    </View>
                    <CardItem style={{ marginTop: 20 }}>
                        <Body style={{ alignItems: 'center' }} >
                            <TouchableOpacity activeOpacity={0.5} onPress={() => {
                                this.props.navigation.navigate('TaskList');
                            }}>
                                <Thumbnail square source={require('./img/icon_dbld.png')} />
                                <Text style={styles.txtMenu}>待办联单</Text>
                            </TouchableOpacity>
                        </Body>
                        <Body style={{ alignItems: 'center' }}>
                            <TouchableOpacity activeOpacity={0.5} onPress={() => {
                                this.props.navigation.navigate('FinishedList');
                            }}>
                                <Thumbnail square source={require('./img/icon_lsld.png')} />
                                <Text style={styles.txtMenu}>历史联单</Text>
                            </TouchableOpacity>
                        </Body>
                        <Body style={{ alignItems: 'center' }}>
                            <TouchableOpacity activeOpacity={0.5} onPress={() => {
                                this.props.navigation.navigate('Setting');
                            }}>
                                <Thumbnail square source={require('./img/icon_set.png')} />
                                <Text style={styles.txtMenu}>系统设置</Text>
                            </TouchableOpacity>
                        </Body>
                    </CardItem>
                </Content>
            </Container >
        );
    }
}

const styles = StyleSheet.create({
    splitLine: {
        borderTopWidth: StyleSheet.hairlineWidth,
        borderTopColor: '#262626',
        width: 50,
        marginTop: 20
    },
    txtMenu: {
        marginTop: 5,
        fontSize: 16
    },
    itemThum: {
        width: 45, height: 45
    },
    title: {
        fontSize: 20,
        marginTop: 20,
        alignSelf: 'center'
    },
    iconNew: {
        fontSize: 40,
        color: '#fff'
    },
    txtNew: {
        fontSize: 18,
        color: '#fff'
    },
    btnNew: {
        width: 150,
        marginTop: 10,
        borderWidth: StyleSheet.hairlineWidth,
        borderColor: '#fff',
        borderStyle: 'solid',
        borderRadius: 15,
        justifyContent: 'center',
        backgroundColor: 'transparent',
    }
});

//make this component available to the app
export default Home;
