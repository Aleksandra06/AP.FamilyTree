// @flow

import normalizeValue from './normalizeValue';

type Transform = { perspective: number }
  | { rotate: string }
  | { rotateX: string }
  | { rotateY: string }
  | { rotateZ: string }
  | { scale: number }
  | { scaleX: number }
  | { scaleY: number }
  | { translateX: number }
  | { translateY: number }
  | { translateZ: number }
  | { skewX: string }
  | { skewY: string }
  | { translate3d: string };

// { scale: 2 } => 'scale(2)'
// { translateX: 20 } => 'translateX(20px)'
const mapTransform = (transform: Transform) => {
  const [type, v] = Object.entries(transform)[0];
  const value = normalizeValue(type, typeof v === 'number' ? v : String(v));
  return `${type}(${value})`;
};

// [1,2,3,4,5,6] => 'matrix3d(1,2,3,4,5,6)'
const convertTransformMatrix = (transformMatrix: Array<number>) => {
  const matrix = transformMatrix.join(',');
  return `matrix3d(${matrix})`;
};

type Style = {
  transform?: ?(string | Array<Transform>),
  transformMatrix?: ?Array<number>,
};

export default function processTransform(style: ?Style) {
  if (style) {
    if (style.transform && Array.isArray(style.transform)) {
      style.transform = style.transform.map(mapTransform).join(' ');
    } else if (style.transformMatrix) {
      style.transform = convertTransformMatrix(style.transformMatrix);
      style.transformMatrix = null;
    }
  }
  return style;
}
