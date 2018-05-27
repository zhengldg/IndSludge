import React, { Component } from 'react';
import { Platform, AsyncStorage, StyleSheet, View, Text, Image, Alert, Dimensions, ActivityIndicator, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Textarea, Separator, List, Picker, Item, Button, Input, ListItem, CheckBox, Body, Grid, Col, Row, Label, Right, Form, Radio, CardItem, Card, Left, ActionSheet } from 'native-base';
import Finished from '../base/finished'
import { get, post } from '../common/request'
import { tokenMgr, userMgr, AsyncStore, Alt, Toast } from '../common/util'
import Modal from "react-native-modal"
import Companys from '../base/companys'
import DatePicker from 'react-native-datepicker'
import Signature from '../base/signature'
import appStyles from '../styles'
import moment from 'moment'
import Icon from 'react-native-vector-icons/FontAwesome';

const width = Dimensions
    .get('window')
    .width, height = Dimensions.get('window').height

export default class RegisterComp extends Component {
    constructor(props) {
        super(props)
        this.state = {
            loading: false,
            form: {
            }
        }
    }

    componentDidMount() {
        var params = this.props.navigation.state.params;
        if (params && params.id) {
            get('eleDuplicate/detail/' + params.id).then(x => {
                if (x.success) {
                    this.setState({
                        form: x.data
                    })
                }
            })
        }
    }

    render() {
        var form = this.state.form;
        return (
            <Container >
                <Content style={styles.formContainer}>
                    {
                        this.state.loading && <ActivityIndicator size="large" style={{ position: 'absolute', alignSelf: 'center' }} />
                    }
                    <Separator >
                        <Text style={{ fontSize: 18, fontWeight: '400' }}>产生单位填写：</Text>
                    </Separator>
                    <Card>
                        <CardItem style={styles.detailItem}>
                            <Text >联单号码：{form.code}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text >是否转移到市外：{form.isOutOfTheCity ? '是' : "否"}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>产生单位：{form.generatedCompanyName}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>联系人\联系电话{form.generatedContact}  {form.generatedTelephoneNumber}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>运输单位：{form.carryingCompanyName}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>联系人\联系电话{form.carryingContact}  {form.carryingTelephoneNumber}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>经营单位：{form.processedCompanyName}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>联系人\联系电话{form.processedContact}  {form.processedTelephoneNumber}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>转运数量\含水率：{form.quantity},({form.quantityPercentageOfMoisture}%)</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>出发时间：{moment(form.departureTime).format('YYYY-MM-DD HH:mm')}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>发运人签名：</Text>
                            <Image resizeMode='stretch' style={{
                                height: 40, margin: 3, width: 140
                            }} source={{ uri: this.state.form.ceneratedOperatorSignBase64 }}></Image>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>发运人手机：{form.generatedOperatorMobilePhone}</Text>
                        </CardItem>

                    </Card>
                    <Separator >
                        <Text style={{ fontSize: 18, fontWeight: '400' }}>经营单位填写：</Text>
                    </Separator>
                    <Card >
                        <CardItem style={styles.detailItem}>
                            <Text>实际接收量\含水率：{form.actualQuantity},({form.actualQuantityPercentageOfMoisture}%)</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>抵达时间：{moment(form.arrivedTime).format('YYYY-MM-DD HH:mm')}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>利用处置方式：{form.handingWays}</Text>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>接收人签名：</Text>
                            <Image resizeMode='stretch' style={{
                                height: 40, margin: 3, width: 140
                            }} source={{ uri: this.state.form.processedOperatorSignBase64 }}></Image>
                        </CardItem>
                        <CardItem style={styles.detailItem}>
                            <Text>接收人手机：{form.processedOperatorMobilePhone}</Text>
                        </CardItem>
                    </Card>
                </Content>
            </Container >);
    }
}

const styles = StyleSheet.create({
    detailItem: {
        height: 35
    },
    container: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
    },
    modalContent: {
        backgroundColor: 'white',
        height: 300,
        width: 340,
        padding: 2,
        borderRadius: 4,
        borderColor: "rgba(0, 0, 0, 0.1)"
    },
    bottomModal: {
        justifyContent: "flex-end",
        margin: 0
    },

    btnGrounp: {
        marginTop: 20,
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center'
    },
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
    saveBtn: { width: 180, marginHorizontal: 10 },
    item: {
    },
    formContainer: {
    },
    btn: {
        marginHorizontal: 10
    }
})
