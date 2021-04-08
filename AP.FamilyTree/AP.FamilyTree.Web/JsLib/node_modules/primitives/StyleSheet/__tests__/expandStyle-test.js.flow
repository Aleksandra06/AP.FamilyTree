// @flow

import expandStyle from '../expandStyle';

describe('StyleSheet/expandStyle', () => {
  test('shortform -> longform', () => {
    const initial = {
      borderStyle: 'solid',
      boxSizing: 'border-box',
      borderBottomColor: 'white',
      borderBottomWidth: 1,
      borderWidth: 0,
      marginTop: 50,
      marginVertical: 25,
      margin: 10,
    };

    const expected = {
      borderBottomStyle: 'solid',
      borderLeftStyle: 'solid',
      borderRightStyle: 'solid',
      boxSizing: 'border-box',
      borderBottomColor: 'white',
      borderTopStyle: 'solid',
      borderTopWidth: '0px',
      borderLeftWidth: '0px',
      borderRightWidth: '0px',
      borderBottomWidth: '1px',
      marginTop: '50px',
      marginBottom: '25px',
      marginLeft: '10px',
      marginRight: '10px',
    };

    expect(expandStyle(initial)).toEqual(expected);
  });

  test('textAlignVertical', () => {
    const initial = {
      textAlignVertical: 'center',
    };

    const expected = {
      verticalAlign: 'middle',
    };

    expect(expandStyle(initial)).toEqual(expected);
  });

  test('flex', () => {
    const value = 10;

    const initial = {
      flex: value,
    };

    const expected = {
      flexGrow: value,
      flexShrink: 1,
      flexBasis: 'auto',
    };

    expect(expandStyle(initial)).toEqual(expected);
  });
});
