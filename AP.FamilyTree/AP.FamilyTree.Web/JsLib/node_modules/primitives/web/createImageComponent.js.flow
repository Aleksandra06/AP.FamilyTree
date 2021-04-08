// @flow

import React from 'react';
import type { PrimitiveProps } from './createDOMElement';
import type { ViewProps } from './createViewComponent';
import type {
  StyleObj,
  StyleSheetFunctions,
} from '../StyleSheet/StyleSheetTypes';

const ImageResizeMode = {
  center: 'center',
  contain: 'contain',
  cover: 'cover',
  none: 'none',
  repeat: 'repeat',
  stretch: 'stretch',
};

export type ImageSource = {
  uri: string,
  width?: number,
  height?: number,
};

export type ImageProps = PrimitiveProps & ViewProps & {
  defaultSource?: ?ImageSource,
  resizeMode?: $Keys<typeof ImageResizeMode>,
  source?: ?ImageSource,
  style?: ?StyleObj,
};

export default function createImageComponent(
  StyleSheet: StyleSheetFunctions,
  View: Class<React.Component<*, ViewProps, *>>,
) {
  const styles = StyleSheet.create({
    initial: {
      backgroundColor: 'transparent',
      backgroundPosition: 'center',
      backgroundRepeat: 'no-repeat',
      backgroundSize: 'cover',
    },
    img: {
      borderWidth: 0,
      height: 'auto',
      maxHeight: '100%',
      maxWidth: '100%',
      opacity: 0,
    },
    children: {
      bottom: 0,
      left: 0,
      position: 'absolute',
      right: 0,
      top: 0,
    },
  });

  const resizeModeStyles = StyleSheet.create({
    center: {
      backgroundSize: 'auto',
      backgroundPosition: 'center',
    },
    contain: {
      backgroundSize: 'contain',
    },
    cover: {
      backgroundSize: 'cover',
    },
    none: {
      backgroundSize: 'auto',
    },
    repeat: {
      backgroundSize: 'auto',
      backgroundRepeat: 'repeat',
    },
    stretch: {
      backgroundSize: '100% 100%',
    },
  });

  return class Image extends React.PureComponent {
    props: ImageProps;

    render() {
      const {
        defaultSource,
        source,
        ...otherProps
      } = this.props;

      const displayImage = resolveAssetSource(defaultSource || source);
      const imageSizeStyle = resolveAssetDimensions(/* !isLoaded ? defaultSource : */ source);
      const backgroundImage = displayImage ? `url("${displayImage}")` : null;
      const originalStyle = StyleSheet.flatten(this.props.style);
      const resizeMode = this.props.resizeMode ||
                         (originalStyle && originalStyle.resizeMode) ||
                         ImageResizeMode.cover;

      // View doesn't support 'resizeMode' as a style
      if (originalStyle) {
        delete originalStyle.resizeMode;
      }

      return <View
        {...otherProps}
        accessibilityRole="img"
        style={[
          styles.initial,
          imageSizeStyle,
          originalStyle,
          !!backgroundImage && { backgroundImage },
          resizeModeStyles[resizeMode],
        ]}
      />;
    }
  };
}

// Helpers

function resolveAssetDimensions(source?: ?ImageSource) {
  if (source && typeof source === 'object') {
    const { height, width } = source;
    return { height, width };
  }
}

function resolveAssetSource(source?: ?ImageSource) {
  return ((source && typeof source === 'object') ? source.uri : source) || null;
}
