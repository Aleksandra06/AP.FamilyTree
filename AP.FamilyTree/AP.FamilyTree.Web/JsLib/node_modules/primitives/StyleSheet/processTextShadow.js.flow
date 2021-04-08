// @flow

import normalizeValue from './normalizeValue';

type Style = {
  textShadowOffset?: ?{
    width: number,
    height: number,
  },
  textShadowRadius?: ?number,
  textShadowColor?: ?string,
  textShadow?: ?string,
};

export default function processTextShadow(style: ?Style) {
  if (style && style.textShadowOffset) {
    const { height, width } = style.textShadowOffset;
    const offsetX = normalizeValue(null, height || 0);
    const offsetY = normalizeValue(null, width || 0);
    const blurRadius = normalizeValue(null, style.textShadowRadius || 0);
    const color = style.textShadowColor || 'currentcolor';

    style.textShadow = `${offsetX} ${offsetY} ${blurRadius} ${color}`;
    style.textShadowColor = null;
    style.textShadowOffset = null;
    style.textShadowRadius = null;
  }
  return style;
}
