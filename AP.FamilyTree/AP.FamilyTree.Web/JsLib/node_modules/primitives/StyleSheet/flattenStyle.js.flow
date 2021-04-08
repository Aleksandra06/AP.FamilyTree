// @flow

import ReactNativePropRegistry from './ReactNativePropRegistry';
import invariant from 'fbjs/lib/invariant';
import isObject from './utils/isObject';
import type { StyleObj } from './StyleSheetTypes';

function getStyle(style) {
  if (typeof style === 'number') {
    return ReactNativePropRegistry.getByID(style);
  }
  return style;
}

export default function flattenStyle(style: ?StyleObj): ?Object {
  if (!style) {
    return undefined;
  }
  invariant(style !== true, 'style may be false but not true');

  if (!Array.isArray(style)) {
    return getStyle(style);
  }

  const result = {};
  for (let i = 0, styleLength = style.length; i < styleLength; ++i) {
    const computedStyle = flattenStyle(style[i]);
    if (computedStyle) {
      merge(result, computedStyle);
    }
  }
  return result;
}

// Helpers

function merge(obj: Object, source: Object) {
  /* eslint no-restricted-syntax: 0 */
  for (const key in source) {
    if (isObject(obj[key]) && isObject(source[key])) {
      obj[key] = merge(obj[key], source[key]);
    } else {
      obj[key] = source[key];
    }
  }

  return obj;
}
