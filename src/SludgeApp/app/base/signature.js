//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, Dimensions, TouchableOpacity, Image } from 'react-native';
import Modal from "react-native-modal";
import Icon from 'react-native-vector-icons/FontAwesome';
import SignatureCapture from 'react-native-signature-capture';
const PropTypes = require('prop-types');
import appStyles from '../styles'

const width = Dimensions
    .get('window')
    .width;
const height = Dimensions
    .get('window')
    .height;

class Signature extends Component {
    static propTypes = {
        ok: PropTypes.func,
        imgBase64: PropTypes.string
    }

    static defaultProps = {
        imgBase64: null
    };

    constructor(props) {
        super(props)
        this.state = {
            visible: false,
            clear: true
        }
    }

    componentWillReceiveProps(nextProps) {
        this.setState({
            visible: nextProps.visible,
        });
    }

    _ok = () => {
        this.refs["sign"].saveImage();
    }

    _cancel = () => {
        if (this.state.clear) {
            this.setState({ visible: false })
        } else {
            this.refs["sign"].resetImage();
            this.setState({
                clear: true
            })
        }
    }

    _onSaveEvent = (result) => {
        const base64String = `data:image/png;base64,${result.encoded}`;
        this.props.ok && this.props.ok(base64String)
    }

    _onDragEvent = () => {
        this.setState({
            clear: false
        })
    }

    _setModelVisible(visible) {
        this.setState({
            visible: visible
        })
    }

    render() {
        return (
            <View style={{ ...this.props.style }}>
                <View style={{ flex: 1, flexDirection: 'row', alignItems: 'center' }}>
                    <View style={[appStyles.grayBorder, { height: 40, margin: 3, width: 140 }]} >
                        <TouchableOpacity style={{ flex: 1 }} activeOpacity={0.5} onPress={() => {
                            this._setModelVisible(true);
                        }} >
                            <Image resizeMode='stretch' style={{
                                flex: 1
                            }} source={{ uri: this.props.imgBase64 }}></Image>
                        </TouchableOpacity>
                    </View>
                    <Icon size={30} style={{ marginLeft: 5 }} type="FontAwesome" name="pencil-square-o" onPress={() => {
                        this._setModelVisible(true);
                    }} />
                </View>

                <Modal
                    onBackdropPress={this._cancel}
                    backdropOpacity={0.5}
                    isVisible={this.state.visible}
                >
                    <View style={styles.container}>
                        <View style={{
                            backgroundColor: 'white',
                            height: 320,
                            width: width - 100,
                            padding: 2,
                            borderRadius: 4,
                            borderColor: "rgba(0, 0, 0, 0.1)"
                        }}>
                            <SignatureCapture
                                style={{ flex: 1 }}
                                ref="sign"
                                onDragEvent={this._onDragEvent}
                                onSaveEvent={this._onSaveEvent}
                                saveImageFileInExtStorage={true}
                                showNativeButtons={false}
                                showTitleLabel={false}
                                viewMode={"portrait"} />
                            <View style={{ borderTopColor: '#d9d9d9', borderTopWidth: StyleSheet.hairlineWidth, borderStyle: 'solid', height: 60, flexDirection: "row", justifyContent: 'flex-end', alignItems: 'center' }}>
                                <View style={styles.btn} >
                                    <Icon.Button name="times-circle" backgroundColor="#3b5998" onPress={this._cancel}>
                                        {this.state.clear ? "关闭" : '清除'}
                                    </Icon.Button>
                                </View>
                                <View style={styles.btn} >
                                    <Icon.Button name="check" backgroundColor="#3b5998" onPress={this._ok}> 确定</Icon.Button>
                                </View>
                            </View>
                        </View>

                    </View>
                </Modal>
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
        marginRight: 20
    }
});

//make this component available to the app
export default Signature;
