//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, Modal, Dimensions } from 'react-native';
import Icon from 'react-native-vector-icons/FontAwesome';
import SignatureCapture from 'react-native-signature-capture';
const PropTypes = require('prop-types');

var width = Dimensions
    .get('window')
    .width

class Signature extends Component {
    static propTypes = {
        ok: PropTypes.func,
        visible: PropTypes.bool
    }

    static defaultProps = {
        visible: false
    };

    constructor(props) {
        super(props)
        this.state = {
            visible: props.visible,
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
        // this.setState({ visible: false })
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
        this.props.ok && this.props.ok(result)
    }

    _onDragEvent = () => {
        this.setState({
            clear: false
        })
    }

    render() {
        return (
            <Modal
                animationType={"slide"}
                transparent={false}
                visible={this.state.visible}
                onRequestClose={() => { alert("Modal has been closed.") }}
            >
                <View style={styles.container}>
                    <Text style={{ color: '#313131', marginTop: 60, marginVertical: 10, textAlign: 'left' }}>请输入签名</Text>
                    <View style={{ width: width * 0.9, height: 200, borderRadius: 3, borderColor: '#5a5a5a', borderWidth: StyleSheet.hairlineWidth}}>
                        <SignatureCapture
                            style={[styles.signature]}
                            ref="sign"
                            onDragEvent={this._onDragEvent}
                            onSaveEvent={this._onSaveEvent}
                            saveImageFileInExtStorage={true}
                            showNativeButtons={false}
                            showTitleLabel={false}
                            viewMode={"portrait"} />
                    </View>
                    <View style={{ height: 80, flexDirection: "row", justifyContent: 'center', alignItems: 'center' }}>
                        <View style={styles.btn} >
                            <Icon.Button name="times-circle" backgroundColor="#3b5998" onPress={this._cancel}>
                                {this.state.clear ? "关闭" : '清除'}
                            </Icon.Button>
                        </View>
                        <View style={styles.btn} >
                            <Icon.Button name="check" backgroundColor="#3b5998" onPress={this._ok}>
                                确定
  </Icon.Button>
                        </View>
                    </View>
                </View>
            </Modal>
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
    signature: {
        flex: 1,
        borderWidth: 1,
        borderColor: 'blue'
    },
    btn: {
        marginHorizontal: 10
    }
});

//make this component available to the app
export default Signature;
