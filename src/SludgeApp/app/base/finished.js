//import liraries
import React, { Component } from 'react';
import { View, Text, StyleSheet, Image, Dimensions } from 'react-native';
import { Container, Header, Content, Card, CardItem, H3, Left, Thumbnail, Button, Input, Icon, Body, Label, Right } from 'native-base';
var width = Dimensions
    .get('window')
    .width

// create a component
class Finished extends Component {
    constructor(props) {
        super(props)
    }

    _clickFinished = () => {
        this.props.clickFinished && this
            .props
            .clickFinished();
    }

    render() {
        const { tip, btnText } = this.props
        return (
            <Container>
                <Content>
                    <Card>
                        <Body style={{ 'flexDirection': 'row', marginTop: 20 }}>
                            <Image source={require('./img/icon_confirm.png')} />
                            <Text style={styles.tip}>{this.props.tip || '操作成功'}</Text>
                        </Body>
                        <Button block rounded style={styles.backBtn} onPress={this._clickFinished}>
                            <Text>{this.props.btnText || '返回'}</Text>
                        </Button>
                    </Card>
                </Content>
            </Container>
        );
    }
}

// define your styles
const styles = StyleSheet.create({
    tip: {
        marginLeft: 10,
        fontSize: 18
    },
    backBtn: { marginHorizontal: 40, marginTop: 40, marginBottom: 20 },
});

//make this component available to the app
export default Finished;
