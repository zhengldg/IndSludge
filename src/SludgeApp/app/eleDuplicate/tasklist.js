import React, { Component } from "react";
import PropTypes from 'prop-types';
import { Text, Image, TouchableOpacity, View, FlatList, Alert, StyleSheet, Dimensions } from "react-native";
import { Container, Header, Content, CardItem, List, ListItem, Left, Body, Right, Thumbnail, Card, H3, Item, Input, Button, Footer } from 'native-base';
import Modal from "react-native-modal";
import { Toast } from '../common/util'
import { get } from '../common/request'
import { isEqual } from 'lodash';
import appStyles from "../styles";
import Icon from 'react-native-vector-icons/FontAwesome';
import moment from 'moment'

var width = Dimensions
    .get('window')
    .width, height = Dimensions
        .get('window')
        .height


class MyListItem extends Component {
    _onPress = () => {
        this.props.onPressItem(this.props.item);
    };

    render() {
        const textColor = this.props.selected ? "red" : "black";
        var item = this.props.item;
        return (
            <TouchableOpacity onPress={this._onPress} activeOpacity={0.5} key={item.id.toString()} >
                <View style={[styles.item]}>
                    <View style={styles.itemLeft}><Image style={{ width: 45, height: 45 }} source={require('../home/img/icon_fqld.png')} /></View>
                    <View style={styles.itemRight}>
                        <Text style={styles.title}>联单号码：{item.code}  </Text>
                        <Text note>发运时间：{moment(item.departureTime).format('YYYY-MM-DD HH:mm')}</Text>
                        <Text note>当前状态：{item.state}  转运数量(吨)：{item.quantity}</Text>
                        {
                            item.state == '经营单位退回'
                            && <Text style={{ color: '#f5222d', textAlign: 'left' }}>原因：{item.backReason}</Text>
                        }
                    </View>
                </View>
            </TouchableOpacity>
        );
    }
}

const comPager = {
    pageSize: 15,
    pageIndex: 1,
    total: 0,
    isRequest: false,
    hasNextPage() {
        return (this.pageIndex - 1) * this.pageSize < this.total;
    }
}

export default class TaskList extends Component {
    static propTypes = {
        parentParams: PropTypes.object,
        isModalVisible: PropTypes.bool
    }

    constructor(props) {
        super(props)
        this.state = {
            isModalVisible: this.props.isModalVisible,
            key: '',
            selected: {},
            data: []
        };
    }

    componentWillReceiveProps(nextProps) {
        if (!isEqual(this.props.parentParams, nextProps.parentParams)) {
            this.setState({
                parentParams: nextProps.parentParams
            })
        }
        if (nextProps.isModalVisible) {
            this.setState({
                isModalVisible: true,
            }, x => {
                this._search();
            });
        }
    }

    shouldComponentUpdate(nextProps, nextState) {
        return true;
    }

    componentDidMount() {
        this._search();
    }

    _keyExtractor = (item, index) => item.id.toString();


    _onPressItem = (item) => {
        this.setState({ selected: item });
        this._editManifest(item);
    };

    _renderItem = ({ item }) => (
        <MyListItem
            id={item.id}
            onPressItem={this._onPressItem}
            selected={item.id == this.state.selected.id}
            item={item}
        />
    );

    _requestData = (type) => {
        var params = Object.assign({ pageSize: comPager.pageSize, pageIndex: comPager.pageIndex, key: this.state.key });
        return get('eleDuplicate/pagedMission', params)
            .then(x => {
                var data = [];
                if (type == 'more') { //加载更多
                    data = this.state.data.concat(x.data);
                } else if (type == 'refresh') { // 刷新
                    data = (x.data || []).concat(this.state.data);
                } else { //查询
                    data = x.data;
                }
                comPager.pageIndex++;
                comPager.total = x.total;
                this.setState({
                    pageIndex: comPager.pageIndex, data: data
                })
            })
            .catch(x => {
                Toast.danger({
                    text: '请求发生错误'
                })
            }).done(x => {
                comPager.isRequest = false;
            })
    }

    _search = () => {
        comPager.pageIndex = 1;
        this._requestData();
    }

    onModalHide = () => {
        this.props.onSelected && this.props.onSelected(this.state.selected);
    }

    _loadMore = (info) => {
        if (this.onEndReachedCalledDuringMomentumFlag === false) {
            console.log(comPager.pageIndex)
            if (comPager.hasNextPage()) {
                this._requestData('more');
            }
            this.onEndReachedCalledDuringMomentumFlag = true;
        }
    }

    onEndReachedCalledDuringMomentum = () => {
        this.onEndReachedCalledDuringMomentumFlag = false;
    }

    _editManifest = (item) => {
        if (item.state == '确认启运') {
            this.props.navigation.navigate('EleDuplicateProcess', { id: item.id });
        } else if (item.state == '经营单位退回') {
            this.props.navigation.navigate('EleDuplicateAddOrEdit', { id: item.id });
        }
    }

    _keyChanged = (x) => {
        this.setState({ key: x });
        this.keyTimer && clearTimeout(this.keyTimer);
        this.keyTimer = setTimeout(y => {
            this._search();
        }, 1000)
    }

    emptyComponent = () => {
        return <View style={{
            alignItems: 'center',
            justifyContent: 'center',
        }}>
            <Text style={{
                fontSize: 16, marginTop: 20
            }}>暂无数据</Text>
        </View>
    }

    _renderDivider = () => {
        return (<View style={[appStyles.itemDivider]} ></View>)
    }

    render() {
        return (
            <View style={styles.container}>
                <Item style={{ paddingHorizontal: 15 }}>
                    <Input placeholder="请输入企业名称\联单号码" value={this.state.key}
                        onChangeText={x => {
                            this._keyChanged(x);
                        }} />
                    <Icon name="search" size={20} onPress={this._search} />
                </Item>
                <FlatList
                onScrollBeginDrag={this.onEndReachedCalledDuringMomentum.bind(this)}
                    data={this.state.data}
                    extraData={this.state}
                    keyExtractor={this._keyExtractor}
                    renderItem={this._renderItem}
                    ItemSeparatorComponent={this._renderDivider}
                    onEndReached={this._loadMore.bind(this)} onEndReachedThreshold={0.01}
                    ListEmptyComponent={this.emptyComponent}
                />
            </View>
        );
    }
}

// define your styles
const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    btn: {
        marginHorizontal: 10
    },
    item: {
        width: width,
        flexDirection: 'row',
        alignItems: 'center',
        padding: 15,
    },
    itemLeft: {
        width: 60
    },
    itemRight: {
        flex: 1,
        flexDirection: 'column',
    }
});

