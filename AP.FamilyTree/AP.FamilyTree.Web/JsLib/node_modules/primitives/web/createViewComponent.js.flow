// @flow

import React from 'react';
import createDOMElement from './createDOMElement';
import type { PrimitiveProps } from './createDOMElement';
import { defaultView } from './DefaultStyles';
import type {
  StyleObj,
  StyleSheetFunctions,
} from '../StyleSheet/StyleSheetTypes';

export type ViewProps = PrimitiveProps & {
  pointerEvents?: 'auto' | 'box-none' | 'box-only' | 'none',
  style?: ?StyleObj,
};

export default function createViewComponent(StyleSheet: StyleSheetFunctions) {
  const styles = StyleSheet.create({
    // https://github.com/facebook/css-layout#default-values
    initial: defaultView,
    flexible: {
      flexShrink: 1,
    },
  });

  return class View extends React.PureComponent {
    props: ViewProps;

    render() {
      const { pointerEvents, style, ...other } = this.props;

      const flattenedStyle = StyleSheet.flatten(style) || {};
      const pointerEventsStyle = pointerEvents && { pointerEvents };

      const props = {
        ...other,
        // TODO: change View to display: inline-block if it's child of Text to behave like React Native iOS
        style: [
          styles.initial,
          style,
          flattenedStyle.flex && styles.flexible, // react-native behaviour
          pointerEventsStyle,
        ],
      };

      return createDOMElement('div', {
        ...props,
        ...StyleSheet.resolve(props),
      });
    }
  };
}
