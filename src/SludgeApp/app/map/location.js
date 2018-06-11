import React, { Component } from 'react';
import { View, Text, StyleSheet, Switch, Platform } from 'react-native';
import { MapView } from 'react-native-amap3d'

export default class Location extends Component {

    static navigationOptions = {
        title: '地图控件',
    }

    state = {
        showsCompass: true,
        showsScale: true,
        showsZoomControls: true,
        showsLocationButton: true,
    }

    onLocation = ({ nativeEvent }) => {
        setTimeout(() => {
            this.mapView.animateTo({
                zoomLevel: 10,
                coordinate: {
                    latitude: nativeEvent.latitude,
                    longitude: nativeEvent.longitude,
                },
            })
        }, 200);
    };

    render() {
        return (
            <View style={StyleSheet.absoluteFill}>
                <MapView ref={ref => this.mapView = ref}
                    onLocation={this.onLocation}
                    locationEnabled={this.state.showsLocationButton}
                    showsCompass={this.state.showsCompass}
                    showsScale={this.state.showsScale}
                    showsLocationButton={this.state.showsLocationButton}
                    showsZoomControls={this.state.showsZoomControls}
                    style={styles.map}
                />
            </View>
        );
    }
}
const styles = StyleSheet.create({
    map: {
        flex: 1,
        ...Platform.select({
            ios: {
                marginBottom: 72,
            },
        }),
    },
    control: {
        alignItems: 'center',
        justifyContent: 'center',
    },
    switch: {
        marginTop: 5,
    },
})