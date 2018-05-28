import React, { Component } from 'react';
import { View, Alert, Text, TouchableOpacity, Dimensions, TouchableHighlight, StyleSheet } from 'react-native';
import { Container, Button, Header, Content, List, ListItem, Left, Body, Right, Switch } from 'native-base';
import Icon from 'react-native-vector-icons/FontAwesome';
import { get } from '../common/request'
var width = Dimensions.get('window').width;
var height = Dimensions.get('window').height;

export default class Setting extends Component {
    constructor(prpos) {
        super(prpos);
        this.state = {
            user: {}
        }
    }

    componentDidMount() {
        get('u/info?username=13202259778')
            .then(x => {
                if (x.success) {
                    this.setState({
                        user: x.data
                    })
                }
            })
    }

    render() {
        return (
            <View style={{ flex: 1, justifyContent: 'flex-start' }} >
                <View style={{ backgroundColor: '#096dd9', height: 180, justifyContent: 'center', alignItems: 'center' }}>
                    <View style={{ alignItems: 'center' }}>
                        <Icon size={66} name="user-circle" color='#fff' style={{ backgroundColor: 'color' }}>
                        </Icon>
                        <Text style={{ color: '#fff', marginTop: 10 }}>{this.state.user.name}</Text>
                    </View>
                </View>
                <View style={{ marginTop: 15 }}>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="lock" size={28} color='#ffa940' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5}  style={styles.right}>
                            <Text style={styles.label}>修改密码</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="map-marker" size={28} color='#1890ff' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5}  style={styles.right}>
                            <Text style={styles.label}>我的位置</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="trash-o" size={28} color='#73d13d' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5}  style={styles.right}>
                            <Text style={styles.label}>清理缓存</Text>
                        </TouchableOpacity>
                    </View>
                    <View style={styles.item}>
                        <View style={styles.left}>
                            <Icon name="info-circle" size={28} color='#006d75' />
                        </View>
                        <TouchableOpacity activeOpacity={0.5} style={styles.right}>
                            <Text style={styles.label}>关于</Text>
                        </TouchableOpacity>
                    </View>
                </View>
                <TouchableOpacity style={{ marginTop: 30, flex: 1, borderColor: '#bfbfbf', borderStyle: 'solid', borderWidth: StyleSheet.hairlineWidth, paddingVertical: 10 }} activeOpacity={0.5}>
                    <Text style={{ color: 'red', fontSize: 18, color: '#40a9ff', alignSelf: 'center' }}>安全退出</Text>
                </TouchableOpacity>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    item: {
        flexDirection: 'row',
        alignItems: 'center',
        paddingTop: 10,
        paddingBottom: 8,
        borderBottomColor: '#bfbfbf', borderStyle: 'solid', borderBottomWidth: StyleSheet.hairlineWidth
    },
    left: {
        width: 42, justifyContent: 'center', alignItems: 'center'
    },
    right: {
        flex: 1
    },
    label: {
        fontSize: 16
    }
})