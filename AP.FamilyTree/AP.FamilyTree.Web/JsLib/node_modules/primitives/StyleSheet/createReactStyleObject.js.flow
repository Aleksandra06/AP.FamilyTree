// @flow

import compose from 'lodash/fp/compose';
import mapValues from 'lodash/mapValues';
import expandStyle from './expandStyle';
import flattenStyle from './flattenStyle';
import processTextShadow from './processTextShadow';
import processTransform from './processTransform';
import isObject from './utils/isObject';
import type { StyleObj } from './StyleSheetTypes';

const runProcessors = compose(
  processTransform,
  processTextShadow,
  expandStyle,
);

function processStyle(style: ?mixed) {
  if (style && isObject(style)) {
    return mapValues(runProcessors(style), processStyle);
  }
  return style;
}

export default function createReactStyleObject(style?: ?StyleObj): Object {
  if (!style) return {};

  const processedStyle = processStyle(flattenStyle(style));
  return processedStyle &&
         typeof processedStyle === 'object' &&
         !Array.isArray(processedStyle)
    ? processedStyle
    : {};
}
