import React, { Component } from "react";
import PropTypes from 'prop-types';
import { Text, TouchableOpacity, View, FlatList, Alert, StyleSheet, TextInput } from "react-native";
import { Container, Header, Content, CardItem, List, ListItem, Left, Body, Right, Thumbnail, Card, H3, Item, Input, Button, Footer } from 'native-base';
import Modal from "react-native-modal";
import { Toast } from '../common/util'
import { get } from '../common/request'
import { isEqual } from 'lodash';
import styles from "./companys.style";
import appStyles from "../styles";
import Icon from 'react-native-vector-icons/FontAwesome';

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
                    <View style={styles.itemLeft}><Icon size={25} name={item.companyType == '产生单位' ? "home" : (item.companyType == '运输单位' ? "car" : "home")} type='FontAwesome' /></View>
                    <View style={styles.itemRight}>
                        <Text style={styles.title}>{item.name}</Text>
                        <Text note>{item.address}  {item.cityName}  {item.contact}</Text>
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
    static propTypes = {
        parentParams: PropTypes.object,
        onSelected: PropTypes.func,
        onCancel: PropTypes.func,
        text: PropTypes.string,
        placeholder: PropTypes.string,
    }

    static defaultProps = {
        isModalVisible: false,
        placeholder: '请选择'
    };

    constructor(props) {
        super(props)
        this.state = {
            isModalVisible: false,
            key: '',
            selected: {},
            data: []
        };
    }

    componentWillReceiveProps(nextProps) {
    }

    shouldComponentUpdate(nextProps, nextState) {
        return true;
    }

    componentDidMount() {
    }

    _keyExtractor = (item, index) => item.id.toString();


    _onPressItem = (item) => {
        this.setModelVisible(false);
        this.props.onSelected && this.props.onSelected(item);
    };

    _onCancel = () => {
        this.setModelVisible(false);
        this.props.onCancel && this.props.onCancel(item);
    }

    _renderItem = ({ item }) => (
        <MyListItem
            id={item.id}
            onPressItem={this._onPressItem}
            selected={item.id == this.state.selected.id}
            item={item}
        />
    );

    setModelVisible(visible) {
        this.setState({
            isModalVisible: visible
        })
        if (visible) {
            this._search();
        }
    }

    onPressPick = () => {
        this.setModelVisible(true);
    }

    _requestData = (type) => {
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
            })
    }

    _search = () => {
        comPager.pageIndex = 1;
        this._requestData();
    }

    _loadMore = () => {
        if (this.onEndReachedCalledDuringMomentumFlag === false) {
            if (comPager.hasNextPage()) {
                this._requestData('more');
                this.onEndReachedCalledDuringMomentumFlag = true;
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
            <Icon name="search" type='FontAwesome' onPress={this._search} />
        </Item>);
    }

    onEndReachedCalledDuringMomentum = () => {
        this.onEndReachedCalledDuringMomentumFlag = false;
    }

    render() {
        return (
            <View style={{ ...this.props.style }}>
                <View style={{ flex: 1, flexDirection: 'row', alignItems: 'center' }}>
                    <TextInput placeholder={this.props.placeholder} value={this.props.text} style={{ flex: 1 }} onFocus={this.onPressPick}></TextInput>
                    <Icon size={20} style={{ margin: 5 }} type="FontAwesome" name="angle-down" onPress={this.onPressPick} ></Icon>
                </View>
                <Modal
                    onBackdropPress={this._onCancel}
                    isVisible={this.state.isModalVisible}
                    backdropOpacity={0.5}
                >
                    <View style={styles.container}>
                        <View style={[styles.modalContent]}>
                            <Item style={{ paddingHorizontal: 15 }}>
                                <Input placeholder="请输入企业名称\联系人" value={this.state.key}
                                    onChangeText={x => {
                                        this.setState({ key: x }, y => {
                                            this._search();
                                        });
                                    }} />
                                <Icon name="search" size={20} onPress={this._search} />
                            </Item>
                            <FlatList style={{ flex: 1 }}
                                onScrollBeginDrag={this.onEndReachedCalledDuringMomentum.bind(this)}
                                data={this.state.data}
                                extraData={this.state}
                                keyExtractor={this._keyExtractor}
                                renderItem={this._renderItem}
                                ItemSeparatorComponent={() => <View style={appStyles.divider} />}
                                onEndReached={this._loadMore.bind(this)} onEndReachedThreshold={0.01}
                                ListEmptyComponent={this.emptyComponent}
                            />
                            <View style={{ borderTopColor: '#d9d9d9', borderTopWidth: StyleSheet.hairlineWidth, borderStyle: 'solid', height: 60, flexDirection: "row", justifyContent: 'flex-end', alignItems: 'center' }}>
                                <View style={{ marginRight: 20 }} >
                                    <Icon.Button name="times" backgroundColor="#3b5998" onPress={this._onCancel}>取消</Icon.Button>
                                </View>
                            </View>
                        </View>
                    </View>
                </Modal>
            </View>

        );
    }
}