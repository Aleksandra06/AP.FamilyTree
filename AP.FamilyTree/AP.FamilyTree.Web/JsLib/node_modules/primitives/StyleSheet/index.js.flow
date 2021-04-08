// @flow

import ReactNativePropRegistry from './ReactNativePropRegistry';
import mapValues from 'lodash/mapValues';
import flatten from '../StyleSheet/flattenStyle';
import type {
  Styles,
  StyleSheet,
} from '../StyleSheet/StyleSheetTypes';

const absoluteFillObject = {
  position: 'absolute',
  left: 0,
  right: 0,
  top: 0,
  bottom: 0,
};
const absoluteFill = ReactNativePropRegistry.register(absoluteFillObject);

function create<S: Styles>(obj: S): StyleSheet<S> {
  return mapValues(obj, (val: Object) =>
    ReactNativePropRegistry.register(val));
}

export default {
  absoluteFillObject,
  absoluteFill,
  hairlineWidth: 1,
  create,
  flatten,
};
