// @flow

import normalizeValue from './normalizeValue';

const emptyObject = {};
const styleShortFormProperties = {
  borderColor: ['borderTopColor', 'borderRightColor', 'borderBottomColor', 'borderLeftColor'],
  borderRadius: ['borderTopLeftRadius', 'borderTopRightRadius', 'borderBottomRightRadius', 'borderBottomLeftRadius'],
  borderStyle: ['borderTopStyle', 'borderRightStyle', 'borderBottomStyle', 'borderLeftStyle'],
  borderWidth: ['borderTopWidth', 'borderRightWidth', 'borderBottomWidth', 'borderLeftWidth'],
  flex: [],
  margin: ['marginTop', 'marginRight', 'marginBottom', 'marginLeft'],
  marginHorizontal: ['marginRight', 'marginLeft'],
  marginVertical: ['marginTop', 'marginBottom'],
  overflow: ['overflowX', 'overflowY'],
  padding: ['paddingTop', 'paddingRight', 'paddingBottom', 'paddingLeft'],
  paddingHorizontal: ['paddingRight', 'paddingLeft'],
  paddingVertical: ['paddingTop', 'paddingBottom'],
  textAlignVertical: [],
  textDecorationLine: ['textDecoration'],
  writingDirection: ['direction'],
};

const alphaSort = (arr: Array<string>) => arr.sort((a, b) => {
  if (a < b) { return -1; }
  if (a > b) { return 1; }
  return 0;
});

const createStyleReducer = originalStyle => {
  const originalStyleProps = Object.keys(originalStyle);

  return (style, prop) => {
    const value = normalizeValue(prop, originalStyle[prop]);
    const longFormProperties = styleShortFormProperties[prop];

    // React Native treats `flex:1` like `flex:1 1 auto`
    if (prop === 'flex') {
      style.flexGrow = value;
      style.flexShrink = 1;
      style.flexBasis = 'auto';
    // React Native accepts 'center' as a value
    } else if (prop === 'textAlignVertical') {
      style.verticalAlign = (value === 'center' ? 'middle' : value);
    } else if (longFormProperties) {
      longFormProperties.forEach((longForm: string) => {
        // the value of any longform property in the original styles takes
        // precedence over the shortform's value
        if (originalStyleProps.indexOf(longForm) === -1) {
          style[longForm] = value;
        }
      });
    } else {
      style[prop] = value;
    }
    return style;
  };
};

type Style = { [key: string]: number | string };

export default function expandStyle(initialStyle: ?Style): Style {
  const style = initialStyle || emptyObject;
  const sortedStyleProps = alphaSort(Object.keys(style));
  const styleReducer = createStyleReducer(style);
  return sortedStyleProps.reduce(styleReducer, {});
}
