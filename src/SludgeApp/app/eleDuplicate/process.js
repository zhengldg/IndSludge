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
import Icon from 'react-native-vector-icons/FontAwesome';

const width = Dimensions
    .get('window')
    .width, height = Dimensions.get('window').height

export default class Register extends Component {
    constructor(props) {
        super(props)
        this.state = {
            finished: false,
            tip: '联单接收成功'
        }
    }

    _goHome = () => {
        this.props.navigation.navigate('Home');
    }

    _finished = (isback) => {
        if (isback) {
            this.setState({ tip: '联单退回成功' }, x => {
                this.setState({ finished: true })
            })
        } else {
            this.setState({ finished: true })
        }
    }

    render() {
        const { navigation } = this.props;
        var finished = this.state.finished;
        return (
            !finished ? <RegisterComp navigation={navigation} finished={this._finished} /> : <Finished btnText='返回到主页面' clickFinished={this._goHome} tip={this.state.tip} />
        );
    }
}

class RegisterComp extends Component {
    constructor(props) {
        super(props)
        this.state = {
            parentParams: {},
            signVisible: false,
            loading: false,
            isModalVisible: false,
            reason: '',
            form: {
                id: null,
                actualQuantity: 0,
                actualQuantityPercentageOfMoisture: 0,
                arrivedTime: '',
                isStored: false,
                processedOperatorSignBase64: null,
                processedOperatorMobilePhone: '',
                handingWays: ''
            }
        }
    }

    componentDidMount() {
        var params = this.props.navigation.state.params;
        if (params && params.id) {
            get('eleDuplicate/process/' + params.id).then(x => {
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
        if (!form.actualQuantity) {
            Toast.warn({
                text: '请填写实际接收量'
            })
            return;
        }
        if (!form.actualQuantity || form.actualQuantity < 0) {
            Toast.warn({
                text: '转运数量必须大于0'
            })
            return;
        }
        if (form.quantityPercentageOfMoisture == null) {
            Toast.warn({
                text: '请填写实际接收量含水率'
            })
            return;
        }
        if (form.actualQuantityPercentageOfMoisture < 0 || form.actualQuantityPercentageOfMoisture > 100) {
            Toast.warn({
                text: '实际接收量含水率在0-100之间'
            })
            return;
        }
        if (!form.arrivedTime) {
            Toast.warn({
                text: '请填写抵达时间'
            })
            return;
        }
        if (form.isStored == '') {
            Toast.warn({
                text: '请选择是否入库'
            })
            return;
        }
        if (!form.handingWays) {
            Toast.warn({
                text: '请选择利用处置方式'
            })
            return;
        }
        if (!form.processedOperatorSignBase64) {
            Toast.warn({
                text: '接收人未签名'
            })
            return;
        }
        if (!form.processedOperatorMobilePhone) {
            Toast.warn({
                text: '请填写接收人手机号码'
            })
            return;
        }

        var self = this;
        Alert.alert(
            '提醒',
            '确定要接收转移联单吗',
            [
                { text: '取消', onPress: () => console.log('Cancel Pressed') },
                {
                    text: '确定', onPress: () => {
                        self.setState({ loading: true });
                        post('eleDuplicate/accept', self.state.form).then(x => {
                            if (x.success) {
                                self.props.finished && self.props.finished();
                            } else {
                            }
                        }).catch(x => {
                        }).done(x => {
                            self.setState({ loading: false });
                        });
                    }
                },
            ],
            { canceLabel: true }
        )
    }

    _back = () => {
        var self = this;
        Alert.alert(
            '提醒',
            '确定要退回该转移联单吗',
            [
                { text: '取消', onPress: () => console.log('Cancel Pressed') },
                {
                    text: '确定', onPress: () => {
                        self.setState({ isModalVisible: true });

                    }
                },
            ],
            { canceLabel: true }
        )
    }

    _updateForm = (field, newVal) => {
        var form = this.state.form;
        form[field] = newVal;
        this.setState({
            form: form
        })
    }

    _cancel = () => {
        this.setState({
            isModalVisible: false
        })
    }

    _ok = () => {
        if (!this.state.reason) {
            Toast.warn({
                text: '请填写退回原因'
            })
            return;
        }

        this.setState({ loading: true })
        post('eleDuplicate/back', { id: this.state.form.id, reason: this.state.reason }).then(x => {
            if (x.success) {
                this.props.finished && this.props.finished(true);
            } else {
                Toast.danger(x.message);
            }
        }).catch(x => {
            Toast.danger({
                text: '提交失败，请检查网络连接是否正常'
            });
        }).done(x => {
            this.setState({ loading: false });
        });
    }

    render() {
        var form = this.state.form;
        return (
            <Container >
                <Modal
                    onBackdropPress={() => { this.setState({ isModalVisible: false, flex: 1 }) }}
                    // onModalHide={this.onModalHide}
                    isVisible={this.state.isModalVisible}
                    backdropOpacity={0.5}
                >
                    <View style={styles.container}>
                        <View style={[styles.modalContent]}>
                            <Content>
                                <Form>
                                    <Textarea rowSpan={6} bordered placeholder="请输入退回原因" onChangeText={(txt) => {
                                        this.setState({
                                            reason: txt
                                        })
                                    }} />
                                </Form>
                                <View style={{ height: 80, flexDirection: "row", justifyContent: 'center', alignItems: 'center' }}>
                                    <View style={styles.btn} >
                                        <Icon.Button name="times-circle" backgroundColor="#3b5998" onPress={this._cancel}>
                                            取消
                                        </Icon.Button>
                                    </View>
                                    <View style={styles.btn} >
                                        <Icon.Button name="check" backgroundColor="#3b5998" onPress={this._ok}>
                                            确定 </Icon.Button>
                                    </View>
                                </View>
                            </Content>
                        </View>
                    </View>
                </Modal>

                <Content style={styles.formContainer}>
                    {
                        this.state.loading && <ActivityIndicator size="large" style={{ position: 'absolute', alignSelf: 'center' }} />
                    }
                    <Separator >
                        <Text style={{ fontSize: 18, fontWeight: '400' }}>产生单位填写：</Text>
                    </Separator>
                    <Card>
                        <CardItem style={{ height: 30 }}>
                            <Text >联单号码：{form.code}</Text>
                        </CardItem>
                        <CardItem style={{ height: 30 }}>
                            <Text>产生单位名称：{form.generatedCompany}</Text>
                        </CardItem>
                    </Card>
                    <Separator >
                        <Text style={{ fontSize: 18, fontWeight: '400' }}>经营单位填写：</Text>
                    </Separator>
                    <Form style={{ marginRight: 15 }}>
                        <Item style={styles.item} >
                            <Label style={styles.label}>实际接收量：</Label>
                            <Input placeholder="请输入实际接收量" value={form.actualQuantity.toString()} keyboardType={'numeric'} onChangeText={(txt) => {
                                this._updateForm('actualQuantity', txt);
                            }} />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>实际接收量含水率(%)：</Label>
                            <Input placeholder="请输入实际接收量含水率" value={form.actualQuantityPercentageOfMoisture.toString()} keyboardType={'numeric'} onChangeText={(txt) => {
                                this._updateForm('actualQuantityPercentageOfMoisture', txt);
                            }} />
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>是否入库：</Label>
                            <Radio selected={form.isStored} onPress={() => {
                                this._updateForm('isStored', !form.isStored)
                            }} style={{ marginVertical: 10 }} />
                        </Item>
                        <Item style={styles.item}>
                            <Label style={styles.label}>抵达时间：</Label>
                            <DatePicker customStyles={{
                                dateInput: appStyles.grayBorder
                            }
                            }
                                date={form.arrivedTime}
                                style={{ width: 200 }}
                                mode="datetime"
                                placeholder="请选择"
                                format="YYYY-MM-DD HH:mm"
                                onDateChange={(date) => { this._updateForm('arrivedTime', date) }}
                            />
                        </Item>
                        <Item style={styles.item}>
                            <Label style={styles.label}>利用处置方式：</Label>
                            <Picker
                                mode="dropdown"
                                placeholder="请选择"
                                placeholderStyle={{ color: "#bfc6ea" }}
                                placeholderIconColor="#007aff"
                                style={[{ flex: 1, backgroundColor: 'transparent' }]}
                                selectedValue={form.handingWays}
                                onValueChange={(v) => {
                                    this._updateForm('handingWays', v);
                                }}
                            >
                                <Picker.Item label="焚烧" value={1} />
                                <Picker.Item label="水泥窑洞" value={2} />
                                <Picker.Item label="堆肥" value={3} />
                                <Picker.Item label="制砖" value={4} />
                                <Picker.Item label="填埋" value={5} />
                                <Picker.Item label="其他" value={10} />
                            </Picker>
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>接收人签名：</Label>
                            <Signature style={{ flex: 1 }} imgBase64={form.ceneratedOperatorSignBase64} ok={(base64) => {
                                this._updateForm('processedOperatorSignBase64', base64)
                            }}>
                            </Signature>
                        </Item>
                        <Item style={styles.item} >
                            <Label style={styles.label}>接收人手机：</Label>
                            <Input placeholder="请输入接收人手机" value={form.processedOperatorMobilePhone} keyboardType={'numeric'} onChangeText={(txt) => {
                                this._updateForm('processedOperatorMobilePhone', txt);
                            }} />
                        </Item>
                    </Form>
                    <View style={styles.btnGrounp}>
                        <Button disabled={this.state.loading} block rounded style={styles.saveBtn} onPress={this._back}>
                            <Text style={{ color: '#fff' }}>退  回</Text></Button>
                        <Button disabled={this.state.loading} block rounded style={styles.saveBtn} onPress={this._save}>
                            <Text style={{ color: '#fff' }}>提  交</Text></Button>
                    </View>
                </Content>
            </Container >);
    }
}

const styles = StyleSheet.create({
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
    saveBtn: { width: 100, marginHorizontal: 10, backgroundColor: '#1890ff' },
    item: {
    },
    formContainer: {
    },
    btn: {
        marginHorizontal: 10
    }
})
