// @flow

import { style as glamorStyle, insertRule } from 'glamor';
import StyleSheet from '../StyleSheet';
import * as css from '../StyleSheet/css';
import createReactStyleObject from '../StyleSheet/createReactStyleObject';
import type { StyleObj } from '../StyleSheet/StyleSheetTypes';

css.getDefaultStyleSheetRules().forEach(rule => insertRule(rule));

type Props = {
  className?: ?string,
  style?: ?StyleObj,
};

function resolve(props: Props) {
  const style = createReactStyleObject(props.style);
  let className = props.className || '';

  // Temp as there's only 'pointerEvents' that needs CSS classes right now
  const prop = 'pointerEvents';
  const value: string = style[prop];
  const replacementClassName = css.getStyleAsHelperClassName(prop, value);
  if (replacementClassName) {
    className += ` ${replacementClassName}`;
    style[prop] = null;
  }

  let glamorClass = glamorStyle(style).toString();
  glamorClass = glamorClass !== '[object Object]' ? glamorClass : null;

  if (glamorClass) {
    className += ` ${glamorClass}`;
  }

  className = className.trim();

  return { className, style: undefined };
}

export default {
  ...StyleSheet,
  resolve,
};
