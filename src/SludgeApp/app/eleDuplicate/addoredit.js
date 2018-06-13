import React, { Component } from 'react';
import { Platform, AsyncStorage, StyleSheet, View, Text, Image, Alert, Dimensions, ActivityIndicator, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Item, Button, Input, ListItem, CheckBox, Body, Grid, Col, Row, Label, Right, Form, Radio, CardItem, Card, Left, ActionSheet } from 'native-base';
import Finished from '../base/finished'
import { get, post } from '../common/request'
import { tokenMgr, userMgr, AsyncStore, Alt, Toast } from '../common/util'
import Modal from "react-native-modal";
import Companys from '../base/companys'
import DatePicker from 'react-native-datepicker'
import Signature from '../base/signature'
import appStyles from '../styles'
import Icon from 'react-native-vector-icons/FontAwesome';

const width = Dimensions
    .get('window')
    .width

export default class Register extends Component {
    constructor(props) {
        super(props)
        this.state = {
            finished: false
        }
    }

    _goHome = () => {
        this.props.navigation.navigate('Home');
    }

    _finished = () => {
        this.setState({ finished: true })
    }

    render() {
        const { navigation } = this.props;
        var finished = this.state.finished;
        return (
            !finished ? <RegisterComp navigation={navigation} finished={this._finished} /> : <Finished btnText='返回到主页面' clickFinished={this._goHome} tip='联单发起成功' />
        );
    }
}

class RegisterComp extends Component {
    constructor(props) {
        super(props)
        this.state = {
            isPickingCarringCompany: false,
            isPickingProcessedCompany: false,
            parentParams: {},
            signVisible: false,
            loading: false,
            isModalVisible: false,
            form: {
                id: null,
                isOutOfTheCity: false,
                carryingCompanyId: '',
                carryingCompanyName: '',
                processedCompanyId: '',
                processedCompanyName: '',
                quantity: 0,
                quantityPercentageOfMoisture: 0,
                ceneratedOperatorSignBase64: null,
                generatedOperatorMobilePhone: '',
                departureTime: '',
            }
        }
    }

    componentDidMount() {
        var params = this.props.navigation.state.params;
        if (params && params.id) {
            get('eleDuplicate/new/' + params.id).then(x => {
                if (x.success) {
                    this.setState({
                        form: x.data
                    })
                }
            })
        }
    }

    _save = () => {
        var form = this.state.form;
        if (!form.carryingCompanyId) {
            Toast.warn({
                text: '请选择运输单位'
            })
            return;
        }
        if (!form.processedCompanyId) {
            Toast.warn({
                text: '请选择经营单位'
            })
            return;
        }
        if (!form.quantity || form.quantity < 0) {
            Toast.warn({
                text: '转运数量必须大于0'
            })
            return;
        }
        if (form.quantityPercentageOfMoisture == null) {
            Toast.warn({
                text: '请填写转运数量含水率'
            })
            return;
        }
        if (form.quantityPercentageOfMoisture < 0 || form.quantityPercentageOfMoisture > 100) {
            Toast.warn({
                text: '转运数量含水率在0-100之间'
            })
            return;
        }
        if (!form.departureTime) {
            Toast.warn({
                text: '请填写出发时间'
            })
            return;
        }
        if (!form.ceneratedOperatorSignBase64) {
            Toast.warn({
                text: '发运人未签名'
            })
            return;
        }
        if (!form.generatedOperatorMobilePhone) {
            Toast.warn({
                text: '请填写发运人手机号码'
            })
            return;
        }

        var self = this;
        Alert.alert(
            '提醒',
            '确定要发起转移联单吗',
            [
                { text: '取消', onPress: () => console.log('Cancel Pressed') },
                {
                    text: '确定', onPress: () => {
                        self.setState({ loading: true });
                        post('eleDuplicate/newELe', self.state.form).then(x => {
                            if (x.success) {
                                self.setState({ loading: false });
                                self.props.finished && self.props.finished();
                            } else {
                                Alert.alert('提交失败', x.message);
                            }
                        }).catch(x => {
                        }).done(x => {
                            self.setState({ loading: false });
                        });
                    }
                },
            ],
            { cancelable: true }
        )
    }

    _submit = () => {
        this.setState({ loading: true });
        post('eleDuplicate/newELe', this.state.form).then(x => {
            if (x.success) {
                this.setState({ loading: false });
                this.props.finished && this.props.finished();
            } else {
                Alert.alert('提交失败', x.message);
            }
        }).catch(x => {
        }).done(x => {
            this.setState({ loading: false });
        });
    }

    _selectCarryingCompany = (item) => {
        var form = this.state.form;
        form.carryingCompanyId = item.id;
        form.carryingCompanyName = item.name;
        this.setState({ form })
    }

    _selectProcessedCompany = (item) => {
        var form = this.state.form;
        form.processedCompanyId = item.id;
        form.processedCompanyName = item.name;
        this.setState({ form })
    }

    _updateForm = (field, newVal) => {
        var form = this.state.form;
        form[field] = newVal;
        this.setState({
            form
        })
    }

    render() {
        var form = this.state.form;
        return (
            <Container >
                <Content style={styles.formContainer}>
                    {
                        this.state.loading && <ActivityIndicator size="large" style={{ position: 'absolute', alignSelf: 'center' }} />
                    }
                    <Form style={{ marginRight: 15 }}>
                        <Item style={styles.item} >
                            <Label style={styles.label}>是否转移到市外：</Label>
                            <Radio selected={form.isOutOfTheCity} onPress={() => {
                                this._updateForm('isOutOfTheCity', !form.isOutOfTheCity)
                            }} style={{ marginVertical: 10 }} />
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>运输单位：</Label>
                            <Companys style={{ flex: 1 }} text={form.carryingCompanyName} parentParams={{ CompanyType: 2 }} onSelected={this._selectCarryingCompany}></Companys>
                        </Item>
                        <Item style={styles.item}  >
                            <Label style={styles.label}>经营单位：</Label>
                            <Companys style={{ flex: 1 }} text={form.processedCompanyName} parentParams={{ CompanyType: 4 }} onSelected={this._selectProcessedCompany}></Companys>
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>转运数量：</Label>
                            <Input placeholder="请输入转运数量" value={form.quantity.toString()} onChangeText={(txt) => {
                                this._updateForm('quantity', txt);
                            }} />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>含水率(%)：</Label>
                            <Input placeholder="请输入含水率" value={form.quantityPercentageOfMoisture.toString()} keyboardType={'numeric'} onChangeText={(txt) => {
                                this._updateForm('quantityPercentageOfMoisture', txt);
                            }} />
                        </Item>
                        <Item style={styles.item}>
                            <Label style={styles.label}>出发时间：</Label>
                            <DatePicker customStyles={{
                                dateInput: appStyles.grayBorder
                            }
                            }
                                date={form.departureTime}
                                style={{ width: 200 }}
                                mode="datetime"
                                placeholder="请选择"
                                format="YYYY-MM-DD HH:mm"
                                onDateChange={(date) => { this._updateForm('departureTime', date) }}
                            />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>发运人签名：</Label>
                            <Signature style={{ flex: 1 }} imgBase64={form.ceneratedOperatorSignBase64}  ok={(base64) => {
                                this._updateForm('ceneratedOperatorSignBase64', base64)
                            }}>
                            </Signature>
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>发运人手机：</Label>
                            <Input placeholder="请输入发运人手机" value={form.generatedOperatorMobilePhone} keyboardType={'numeric'} onChangeText={(txt) => {
                                this._updateForm('generatedOperatorMobilePhone', txt);
                            }} />
                        </Item>
                    </Form>
                    <View style={styles.btnGrounp}>
                        <Button disabled={this.state.loading} block rounded style={styles.saveBtn} onPress={this._save}>
                            <Text style={{ color: '#fff' }}>提  交</Text></Button>
                    </View>
                </Content>
            </Container>);
    }
}

const styles = StyleSheet.create({
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
    saveBtn: { width: 100, marginHorizontal: 10,backgroundColor: '#1890ff' },
    item: {
    },
    formContainer: {
    },
})
