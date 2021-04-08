// @flow

import React from 'react';
import createDOMElement from './createDOMElement';
import type { PrimitiveProps } from './createDOMElement';
import { defaultText } from './DefaultStyles';
import type {
  StyleObj,
  StyleSheetFunctions,
} from '../StyleSheet/StyleSheetTypes';

export type TextProps = PrimitiveProps & {
  children?: ?React.Element<*>,
  // accessibilityRole?: 'heading' | 'link', // TODO: should really be 'heading' | 'link' but not possible with '&'. Waiting for https://github.com/facebook/flow/issues/1326
  numberOfLines?: number,
  onPress?: Function,
  onClick?: Function,
  selectable?: number,
  style?: ?StyleObj,
};

export default function createTextComponent(StyleSheet: StyleSheetFunctions) {
  const styles = StyleSheet.create({
    initial: defaultText,
    notSelectable: {
      userSelect: 'none',
    },
    singleLineStyle: {
      maxWidth: '100%',
      overflow: 'hidden',
      textOverflow: 'ellipsis',
      whiteSpace: 'nowrap',
    },
  });

  return class Text extends React.PureComponent {
    props: TextProps;

    render() {
      const {
        children,
        numberOfLines,
        onPress,
        onClick,
        selectable = true,
        style,
        ...other
      } = this.props;

      // transform {'\n'} into <br />
      const mappedChildren: React.Element<*> = React.Children.map(
        children,
        (child: React.Element<*>) => (child === '\n' ? <br /> : child),
      );

      const props = {
        ...other,
        children: mappedChildren,
        onClick: onPress || onClick,
        style: [
          styles.initial,
          style,
          !selectable && styles.notSelectable,
          numberOfLines === 1 && styles.singleLineStyle,
        ],
      };

      return createDOMElement('span', {
        ...props,
        ...StyleSheet.resolve(props),
      });
    }
  };
}
