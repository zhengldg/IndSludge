import React, { Component } from "react";
import PropTypes from 'prop-types';
import { Text, TouchableOpacity, View, FlatList, Alert, StyleSheet } from "react-native";
import { Container, Header, Icon, Content, CardItem, List, ListItem, Left, Body, Right, Thumbnail, Card, H3, Item, Input, Button, Footer } from 'native-base';
import Modal from "react-native-modal";
import { Toast } from '../common/util'
import { get } from '../common/request'
import { isEqual } from 'lodash';
import styles from "./companys.style";
import appStyles from "../styles";


class MyListItem extends Component {
    _onPress = () => {
        this.props.onPressItem(this.props.item);
    };

    render() {
        const textColor = this.props.selected ? "red" : "black";
        var item = this.props.item;
        return (
            <TouchableOpacity onPress={this._onPress} activeOpacity={0.5} key={item.id} >
                <View style={[styles.item]}>
                    <View style={styles.itemLeft}><Icon name="ios-home-outline" /></View>
                    <View style={styles.itemRight}>
                        <Text style={styles.title}>{item.name}</Text>
                        <Text note>{item.address}  {item.contractor}</Text>
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
    hasNextPage() {
        return (this.pageIndex - 1) * this.pageSize < this.total;
    }
}

export default class Companys extends Component {
    isRequest = false;

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
        // if (!isEqual(nextProps, this.props)) {
        //     return true;
        // }
        // return false;
    }

    componentDidMount() {
        this._search();
    }

    _keyExtractor = (item, index) => item.id;


    _onPressItem = (item) => {
        this.setState({ selected: item, isModalVisible: false });
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
        this.isRequest = true;
        var parentParams = this.props.parentParams;
        var params = Object.assign({ pageSize: comPager.pageSize, pageIndex: comPager.pageIndex, key: this.state.key }, parentParams);
        return get('company/pagedCompany', params)
            .then(x => {
                var data = [];
                if (type == 'more') { //加载更多
                    data = this.state.data.concat(x.data.items);
                } else if (type == 'refresh') { // 刷新
                    data = (x.data.items || []).concat(this.state.data);
                } else { //查询
                    data = x.data.items;
                }
                comPager.pageIndex++;
                comPager.total = x.data.total;
                this.setState({
                    pageIndex: comPager.pageIndex, data: data
                })
            })
            .catch(x => {
                Toast.danger({
                    text: '请求发生错误'
                })
            }).done(x => {
                this.isRequest = false;
            })
    }

    _search = () => {
        comPager.pageIndex = 1;
        this._requestData();
    }

    onModalHide = () => {
        this.props.onSelected && this.props.onSelected(this.state.selected);
    }

    _loadMore = () => {
        if (!this.isRequest) {
            if (comPager.hasNextPage()) {
                this._requestData('more');
            }
        }
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

    HeaderComponent = () => {
        return (<Item >
            <Input placeholder="请输入企业名称\联系人" value={this.state.key}
                onChangeText={x => {
                    this.setState({ key: x });
                }} onKeyPress={
                    x => {

                    }
                } />
            <Icon name="ios-search" onPress={this._search} />
        </Item>);
    }


    render() {
        return (
            <Modal
                onBackdropPress={() => { this.setState({ isModalVisible: false, flex: 1 }) }}
                onModalHide={this.onModalHide}
                isVisible={this.state.isModalVisible}
                backdropOpacity={0.5}
            >
                <View style={styles.container}>
                    <View style={[styles.modalContent]}>
                        <Item >
                            <Input placeholder="请输入企业名称\联系人" value={this.state.key}
                                onChangeText={x => {
                                    this.setState({ key: x }, y => {
                                        this._search();
                                    });
                                }} />
                            <Icon name="ios-search" onPress={this._search} />
                        </Item>
                        <FlatList
                            data={this.state.data}
                            extraData={this.state}
                            keyExtractor={this._keyExtractor}
                            renderItem={this._renderItem}
                            ItemSeparatorComponent={() => <View style={appStyles.divider} />}
                            onEndReached={this._loadMore} onEndReachedThreshold={1.5}
                            ListEmptyComponent={this.emptyComponent}
                        />
                    </View>
                </View>
            </Modal>
        );
    }
}