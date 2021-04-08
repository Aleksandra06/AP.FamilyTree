// @flow

import StyleSheet from './web/StyleSheet';
import type { StyleObj } from './StyleSheet/StyleSheetTypes';
import createViewComponent from './web/createViewComponent';
import type { ViewProps } from './web/createViewComponent';
import createTextComponent from './web/createTextComponent';
import type { TextProps } from './web/createTextComponent';
import createImageComponent from './web/createImageComponent';
import type { ImageProps } from './web/createImageComponent';

export {
  StyleSheet,
  createViewComponent,
  createTextComponent,
  createImageComponent,
};

export const View = createViewComponent(StyleSheet);
export const Text = createTextComponent(StyleSheet);
export const Image = createImageComponent(StyleSheet, View);

export type {
  StyleObj,
  ViewProps,
  TextProps,
  ImageProps,
};
