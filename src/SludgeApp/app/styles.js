import React, { Component } from 'react';
import { StyleSheet, Dimensions } from 'react-native'
var width = Dimensions
    .get('window')
    .width, height = Dimensions
        .get('window')
        .height

export default StyleSheet.create({
    divider: {
        borderBottomColor: '#d9d9d9', borderBottomWidth: StyleSheet.hairlineWidth, borderStyle: 'solid'
    },
    itemDivider: {
        height: 1, backgroundColor: '#d9d9d9', flex: 1,
    },
    grayBg: {
        backgroundColor: '#e8e8e8'
    },
    row: {
        flexDirection: 'row'
    },
    col: {
        flexDirection: 'column'
    },
    grayBorder: {
        borderWidth: StyleSheet.hairlineWidth, borderColor: '#d9d9d9', borderStyle: 'solid'
    },
})
