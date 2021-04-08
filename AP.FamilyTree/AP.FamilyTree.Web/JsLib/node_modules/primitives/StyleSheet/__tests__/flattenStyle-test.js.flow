// @flow

import flattenStyle from '../flattenStyle';

describe('StyleSheet/flattenStyle', () => {
  it('should merge style objects', () => {
    const style1 = { opacity: 1 };
    const style2 = { order: 2 };
    expect(flattenStyle([style1, style2]))
      .toEqual({ opacity: 1, order: 2 });
  });

  it('should override style properties', () => {
    const style1 = { backgroundColor: '#000', order: 1 };
    const style2 = { backgroundColor: '#023c69', order: null };
    expect(flattenStyle([style1, style2]))
      .toEqual(style2);
  });

  it('should overwrite properties with `undefined`', () => {
    const style1 = { backgroundColor: '#000' };
    const style2 = { backgroundColor: undefined };
    expect(flattenStyle([style1, style2]))
      .toEqual(style2);
  });

  it('should not fail on falsy values', () => {
    expect(() => flattenStyle([null, false, undefined]))
      .not.toThrow();
  });

  it('should recursively flatten arrays', () => {
    const style1 = { order: 2 };
    const style2 = { opacity: 1 };
    const style3 = { order: 3 };
    const flatStyle = flattenStyle([null, [], [style1, style2], style3]);
    expect(flatStyle)
      .toEqual({ opacity: 1, order: 3 });
  });

  it('should merge nested styles', () => {
    const style1 = {
      backgroundColor: '#000',
      order: 2,
      ':hover': {
        backgroundColor: 'tomato',
        order: 3,
      },
      '@media(min-width: 300px)': {
        color: 'green',
        ':hover': {
          backgroundColor: 'turquoise',
          color: 'yellow',
        },
      },
    };
    const style2 = {
      order: 1,
      ':hover': {
        order: 2,
      },
      '@media(min-width: 300px)': {
        color: 'blue',
        ':hover': {
          color: 'gold',
        },
      },
    };
    expect(flattenStyle([style1, style2]))
      .toEqual({
        backgroundColor: '#000',
        order: 1,
        ':hover': {
          backgroundColor: 'tomato',
          order: 2,
        },
        '@media(min-width: 300px)': {
          color: 'blue',
          ':hover': {
            backgroundColor: 'turquoise',
            color: 'gold',
          },
        },
      });
  });
});
