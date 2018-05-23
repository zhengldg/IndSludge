import { StyleSheet, Dimensions } from "react-native";
const width = Dimensions
    .get('window')
    .width;
const height = Dimensions
    .get('window')
    .height;
export default StyleSheet.create({
    title: {
        fontSize: 16,
        fontWeight: '600'
    },
    container: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
    },
    modalContent: {
        backgroundColor: 'white',
        height: height - 200,
        width: width - 100,
        padding: 2,
        borderRadius: 4,
        borderColor: "rgba(0, 0, 0, 0.1)"
    },
    bottomModal: {
        justifyContent: "flex-end",
        margin: 0
    },
    item: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: 10,
    },
    itemLeft: {
        width: 30
    },
    itemRight: {
        flexDirection: 'column',
    }
});