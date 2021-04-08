// @flow

import createReactStyleObject from '../createReactStyleObject';

describe('StyleSheet/createReactStyleObject', () => {
  it('converts ReactNative style to ReactDOM style', () => {
    expect(createReactStyleObject({
      display: 'flex',
      marginVertical: 0,
      opacity: 0,
      '@media(min-width: 300px)': {
        paddingHorizontal: 0,
        ':hover': {
          marginHorizontal: '2rem',
        },
      },
    })).toEqual({
      display: 'flex',
      marginTop: '0px',
      marginBottom: '0px',
      opacity: 0,
      '@media(min-width: 300px)': {
        paddingLeft: '0px',
        paddingRight: '0px',
        ':hover': {
          marginLeft: '2rem',
          marginRight: '2rem',
        },
      },
    });
  });
});
