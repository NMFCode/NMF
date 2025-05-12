package com.example.util;
import java.util.Iterator;
public class IntRange implements Iterable<Integer> {
  int from;
  int to;
  int step;
  boolean infinite;
  
  public IntRange(int from,  int to,  int step){
    this.from = from;
    this.to = to;
    this.step = step;
    infinite = false;
  }
  public IntRange(int from,  int to){
    this(from,to,1);
  }
  public IntRange(int from){
    this(from,0);
    infinite = true;
  }
  public IntRange(){
    this(1);
  }
  @Override public Iterator<Integer> iterator(){
    return new Iter(from);
  }
  private class Iter implements Iterator<Integer>{
    int from;
    Iter(int from){this.from = from;}
    @Override public boolean hasNext(){
      return infinite ||  (from<= to && step>=0) ||  (from>= to && step<=0);
    }
    @Override public Integer next(){
      int result = from;
      from = from+step;
      return result;
    }
 }
}